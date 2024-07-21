using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;

namespace api.DTOs.Comment
{
    public class UpdateCommentRequestDto : ICommentDto
    {
        [Required]
        [MinLength(5,ErrorMessage = "Title must be more than 5 characters")]
        [MaxLength(280, ErrorMessage = "Title can't be over 280 characters")]
        public string Title { get; set; } = String.Empty;
        [Required]
        [MinLength(5,ErrorMessage = "Comment must be more than 5 characters")]
        [MaxLength(280, ErrorMessage = "Comment can't be over 280 characters")]
        public string Content { get; set; } = String.Empty;
    }
}