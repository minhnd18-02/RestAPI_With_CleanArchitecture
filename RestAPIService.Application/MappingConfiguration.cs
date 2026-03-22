using AutoMapper;
using RestAPIService.Application.ViewModels.RegisterModel;
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
        }
    }
}
