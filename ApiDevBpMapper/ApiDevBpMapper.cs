using ApiDevBP.Entities;
using ApiDevBP.Models.DTO;
using AutoMapper;
using System.CodeDom;

namespace ApiDevBP.ApiDevBpMapper
{
    public class ApiDevBpMapper : Profile
    {
        public ApiDevBpMapper()
        {
            CreateMap<UserEntity, UserDto>().ReverseMap(); 
        }
    }
}
