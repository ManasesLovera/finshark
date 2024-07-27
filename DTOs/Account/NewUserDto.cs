using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Account
{
    public class NewUserDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string? Email { get; set; }
        [Required]
        public string? Token { get; set; }
    }
}