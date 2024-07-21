using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;

namespace api.DTOs.Comment
{
    public class CreateCommentDto : ICommentDto
    {
        [Required]
        [MinLength(5,ErrorMessage = "El titulo debe tener 5 caracteres o mas")]
        [MaxLength(280, ErrorMessage = "El titulo no puede contener mas de 280 caracteres")]
        public string Title { get; set; } = String.Empty;
        [Required]
        [MinLength(5,ErrorMessage = "El contenido debe tener 5 caracteres o mas")]
        [MaxLength(280, ErrorMessage = "El contenido no puede contener mas de 280 caracteres")]
        public string Content { get; set; } = String.Empty;
    }
}