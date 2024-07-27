using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Account
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is missing")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Email address is missing")]
        [EmailAddress(ErrorMessage = "Email address is not valid")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is missing")]
        public string? Password { get; set; }
    }
}