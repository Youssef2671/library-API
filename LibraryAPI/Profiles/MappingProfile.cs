using AutoMapper;
using LibraryAPI.Models;
using LibraryAPI.DTOs;

namespace LibraryAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // العرض: من الموديل للـ DTO (Source -> Destination)
            CreateMap<Author, AuthorDTO>();

            // الـ AutoMapper ذكي وهيفهم لوحده إن AuthorName اللي في الـ DTO 
            // بتيجي من Author.Name اللي جوه الـ Book
            CreateMap<Book, BookDTO>();

            // الإضافة: من الـ DTO للموديل (تجاهلنا مسار الصورة)
            CreateMap<BookCreateDTO, Book>()
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore());
            CreateMap<Author, AuthorDTO>();
            CreateMap<AuthorCreateDTO, Author>();
        }
    }
}