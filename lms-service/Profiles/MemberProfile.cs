using System;
using AutoMapper;
using lms_data.Entities;
using lms_service.Models;

namespace lms_service.Profiles
{
	public class MemberProfile : Profile
    {
		public MemberProfile()
		{
            CreateMap<Member, MemberEntity>()
               .ReverseMap();
        }
	}
}

