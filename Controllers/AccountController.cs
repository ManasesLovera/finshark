using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try 
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                AppUser appUser = _mapper.Map<AppUser>(registerDto);
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password!);

                if(createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    if(roleResult.Succeeded)
                        return Ok(
                            new NewUserDto
                            {
                                Username = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.CreateToken(appUser)
                            }
                        );
                    else
                        return StatusCode(409, roleResult.Errors);
                }
                else
                    return StatusCode(409, createdUser.Errors);
                
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode:500);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.Username);

            if(user == null)
                return Unauthorized("Username no found and/or password incorrect!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);

            if(!result.Succeeded)
                return Unauthorized("Username no found and/or password incorrect!");

            return Ok(
                new NewUserDto {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                }
            );
        }
    }
}