using AutoMapper;
using BookStore.Contracts;
using BookStore.Models;
using BookStore.DTOs;

namespace BookStore.Mapping
{
    public class BookMapper : Profile
    {
        public BookMapper()
        {
            CreateMap<CreateBookRequest, Book>()
            .ForMember(dest => dest.BookId, opt => opt.Ignore()); 

            CreateMap<Book, BookResponseDTO>();          

            CreateMap<UpdateBookRequest, Book>()
            .ForMember(dest => dest.BookId, opt => opt.Ignore());
        }
    }
}