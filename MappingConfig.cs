using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.DTOs.Stock;
using api.Models;
using AutoMapper;

namespace api
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Stock, StockDto>().ReverseMap();
            CreateMap<CreateStockRequest, Stock>().ReverseMap();
            CreateMap<UpdateStockRequestDto, StockDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
        }
        
    }
}