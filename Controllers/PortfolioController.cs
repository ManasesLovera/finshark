using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepository, IPortfolioRepository portfolioRepository)
        {
            _userManager = userManager;
            _stockRepo = stockRepository;
            _portfolioRepo = portfolioRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser!);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(string symbol)
        {
            string? username = User.FindFirst(ClaimTypes.GivenName)?.Value;
            AppUser? appUser = await _userManager.FindByNameAsync(username!);
            Stock? stock = await _stockRepo.GetBySymbolAsync(symbol);

            if(stock == null)
                return NotFound("Stock not found");

            if(appUser == null)
                return NotFound("User not found");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
                return BadRequest("The user already has this stock in their portfolio");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id,
            };

            await _portfolioRepo.CreateAsync(portfolioModel);

            if(portfolioModel == null)
                return StatusCode(500, "Could not create");
            else
                return Created();
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string symbol)
        {
            var username = User.FindFirst(ClaimTypes.GivenName)?.Value;
            var appUser = await _userManager.FindByNameAsync(username!);

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser!);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if(filteredStock.Count() == 1)
                await _portfolioRepo.DeleteAsync(appUser!, symbol);
            else return BadRequest("Stock not in your portfolio");

            return Ok("Deleted successfully");
        }
    }
}