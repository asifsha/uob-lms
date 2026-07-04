using System;
using AutoMapper;
using lms_data.Entities;
using lms_service.Models;

namespace lms_service.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookEntity>()
                .ReverseMap();
        }
    }
}

