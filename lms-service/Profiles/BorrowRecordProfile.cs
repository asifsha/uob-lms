using System;
using AutoMapper;
using lms_data.Entities;
using lms_service.Models;

namespace lms_service.Profiles
{
	public class BorrowRecordProfile : Profile
    {
		public BorrowRecordProfile()
		{
            CreateMap<BorrowRecord, BorrowRecordEntity>()
               .ReverseMap();
        }
	}
}

