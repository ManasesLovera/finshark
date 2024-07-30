using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, 
                                 IMapper mapper, IValidator<ICommentDto> validator)
        {
            _commentRepo = commentRepository;
            _stockRepo = stockRepository;
            _mapper = mapper;
            _validator = validator;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            return Ok(comments);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(_mapper.Map<CommentDto>(comment));
        }
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            try {
                var result = await _validator.ValidateAsync(commentDto);
                if(!result.IsValid)
                    return BadRequest(result.Errors);

                if(!await _stockRepo.StockExists(stockId))
                    return BadRequest("Stock does not exist");

                var commentModel = _mapper.Map<Comment>(commentDto);
                commentModel.StockId = stockId;
                await _commentRepo.CreateAsync(commentModel);
                return CreatedAtAction(nameof(GetById), new {id = commentModel}, _mapper.Map<CommentDto>(commentModel));
            }
            catch (Exception ex) {
                return Problem(ex.Message, statusCode: 500);
            }
        }
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