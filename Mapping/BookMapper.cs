using AutoMapper;
using BookStore.Contracts;
using BookStore.Models;
using BookStore.DTOs;
using BookStore.HandleResponse;
using BookStore.Services.Interfaces;
using BookStore.Data.Cart;

namespace BookStore.Mapping
{
    public class BookMapper : Profile
    {
        public BookMapper()
        {
            CreateMap<CreateBookRequest, Book>()
            .ForMember(dest => dest.BookId, opt => opt.Ignore()); 

            CreateMap<Book, BookResponseDTO>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)));

            CreateMap<UpdateBookRequest, Book>()
            .ForMember(dest => dest.BookId, opt => opt.Ignore());

            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<Order, OrderResponseDTO>().ReverseMap();

            CreateMap<ShoppingCart, ShoppingCartDTO>();

        }
    }
}