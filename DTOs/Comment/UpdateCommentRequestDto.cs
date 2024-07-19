using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Comment
{
    public class UpdateCommentRequestDto
    {
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
    }
}