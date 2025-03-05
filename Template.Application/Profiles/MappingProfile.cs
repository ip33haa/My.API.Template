using AutoMapper;
using Template.Application.DTOs;
using Template.Domain.Entities;

namespace Template.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();

            CreateMap<User, RegisterDto>().ReverseMap();

            CreateMap<Notification, NotificationDTO>().ReverseMap();

            CreateMap<SIPOC, SIPOCDto>().ReverseMap();

            CreateMap<Customer, CustomerDto>().ReverseMap();

            CreateMap<Supplier, SupplierDto>().ReverseMap();
        }
    }
}
