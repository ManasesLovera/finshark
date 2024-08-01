using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<ICommentDto> _validator;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, 
                                 IMapper mapper, IValidator<ICommentDto> validator, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepository;
            _stockRepo = stockRepository;
            _mapper = mapper;
            _validator = validator;
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            return Ok(comments);
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
                return NotFound();
            CommentDto commentDto = _mapper.Map<CommentDto>(comment);
            commentDto.CreatedBy = comment.AppUser!.UserName!;
            return Ok(commentDto);
        }
        [Authorize]
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            try {
                var result = await _validator.ValidateAsync(commentDto);
                if(!result.IsValid)
                    return BadRequest(result.Errors);

                if(!await _stockRepo.StockExists(stockId))
                    return BadRequest("Stock does not exist");

                var username = User.FindFirst(ClaimTypes.GivenName)!.Value;
                var appUser = await _userManager.FindByNameAsync(username);

                var commentModel = _mapper.Map<Comment>(commentDto);
                commentModel.StockId = stockId;
                commentModel.AppUserId = appUser!.Id;
                await _commentRepo.CreateAsync(commentModel);
                return CreatedAtAction(nameof(GetById), new {id = commentModel.Id}, _mapper.Map<CommentDto>(commentModel));
            }
            catch (Exception ex) {
                return Problem(ex.Message, statusCode: 500);
            }
        }
        [Authorize]
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            try
            {
                var result = await _validator.ValidateAsync(updateDto);
                if(!result.IsValid)
                    return BadRequest(result.Errors);

                var comment = await _commentRepo.UpdateAsync(id,_mapper.Map<Comment>(updateDto));

                if(comment == null)
                    return NotFound("Comment not found");
                
                return Ok(_mapper.Map<CommentDto>(comment));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode:500);
            }            
        }
        [Authorize]
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {               
            var comment = await _commentRepo.DeleteAsync(id);
            if(comment == null)
                return NotFound("Comment not found");
            
            return Ok(_mapper.Map<CommentDto>(comment));
        }
    }
}