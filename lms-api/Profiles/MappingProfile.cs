using System;
using AutoMapper;
using lms_api.Models.Requests;
using lms_service.Models;

namespace lms_api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookRequest, Book>();
            CreateMap<MemberRequest, Member>();
        }
    }
}

