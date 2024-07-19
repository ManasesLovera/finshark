using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Stock
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol can't be over 10 charanters")]
        [MinLength(2, ErrorMessage = "Symbol must be 2 or over")]
        public string Symbol { get; set; } = String.Empty;
        [Required]
        [MaxLength(20, ErrorMessage = "Company name can't be over 20 charanters")]
        public string CompanyName { get; set; } = String.Empty;
        [Required]
        [Range(1, 1000000000)]
        public decimal Purchase { get; set; }
        [Required]
        [Range(0.001,100)]
        public decimal LastDiv { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Industry can't be over 20 characters")]
        public string Industry { get; set; } = String.Empty;
        [Required]
        [Range(1,5000000000)]
        public long MarketCap { get; set; }
    }
}