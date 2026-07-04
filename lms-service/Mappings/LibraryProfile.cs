using System;
using AutoMapper;
using lms_data.Entities;
using lms_service.Models;

namespace lms_service.Mappings
{
    public class LibraryProfile : Profile
    {
        public LibraryProfile()
        {
            // Map BookEntity <-> BookModel
            CreateMap<BookEntity, Book>().ReverseMap();

            // You can add more here
            // CreateMap<MemberEntity, MemberModel>().ReverseMap();
            // CreateMap<LibraryEntity, LibraryModel>().ReverseMap();
        }
    }
}

