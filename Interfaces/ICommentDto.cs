using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface ICommentDto
    {
        public string Title { get; set; }
        public string Content { get; set; } 
    }
}