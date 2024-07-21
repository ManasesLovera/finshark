using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using AutoMapper;
using FluentValidation;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateStockRequest> _validator;

        public StockController(IStockRepository stockRepository, ApplicationDbContext context, 
                               IMapper mapper,IValidator<CreateStockRequest> validator)
        {
            _stockRepo = stockRepository;
            _context = context;
            _mapper = mapper;
            _validator = validator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            try {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                var stocks = await _stockRepo.GetAllAsync(query);
                var stocksDto = stocks.Select(s => _mapper.Map<StockDto>(s));
                return Ok(stocksDto);
            }
            catch (Exception ex) {
                return Problem(ex.Message, statusCode: 500);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepo.GetByIdAsync(id);
            if(stock == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<StockDto>(stock));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequest stockDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = _mapper.Map<Stock>(stockDto);
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id}, _mapper.Map<StockDto>(stockModel));
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateStockRequestDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepo.UpdateAsync(id, updateStockRequestDto);

            if(stockModel == null)
                return NotFound();

            return Ok(_mapper.Map<StockDto>(stockModel));
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepo.DeleteAsync(id);

            if(stock == null)
                return NotFound();

            return NoContent();
        }
    }
}