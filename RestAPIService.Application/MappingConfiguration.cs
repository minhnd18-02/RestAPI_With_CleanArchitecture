using AutoMapper;
using RestAPIService.Application.ViewModels.ProductModel;
using RestAPIService.Application.ViewModels.RegisterModel;
using RestAPIService.Application.ViewModels.UserModel;
using RestAPIService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<User, RegisterRequest>().ReverseMap();
            CreateMap<User, RegisterReponse>().ReverseMap();
            CreateMap<User, UpdateUserRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<Product, ProductRequest>().ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();
        }
    }
}
