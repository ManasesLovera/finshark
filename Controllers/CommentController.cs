using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepo = commentRepository;
            _stockRepo = stockRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllAsync();
            return Ok(comments);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id, IMapper _mapper)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
                return NotFound();
            return Ok(_mapper.Map<CommentDto>(comment));
        }
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create(IMapper _mapper,[FromRoute] int stockId, CreateCommentDto commentDto)
        {
            try {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

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
        public async Task<IActionResult> Update(IMapper _mapper,[FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.UpdateAsync(id,_mapper.Map<Comment>(updateDto));

            if(comment == null)
                return NotFound("Comment not found");
            
            return Ok(_mapper.Map<CommentDto>(comment));
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(IMapper _mapper, [FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var comment = await _commentRepo.DeleteAsync(id);
            if(comment == null)
                return NotFound("Comment not found");
            
            return Ok(_mapper.Map<CommentDto>(comment));
        }
    }
}