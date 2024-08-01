using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;

namespace api.DTOs.Comment
{
    public class CommentDto : ICommentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = String.Empty;
        public int? StockId { get; set; }
    }
}