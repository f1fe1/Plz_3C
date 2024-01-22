using AutoMapper;
using BookCollection.Dtos;
using BookCollection.Models;

namespace BookCollection.Profiles
{
    //успадковує від класу Profile з бібліотеки AutoMapper.
    // Цей клас використовується для конфігурування відображень між типами об'єктів.
    public class BookProfile : Profile
    {
        //У конструкторі визначені конфігурації відображень для різних типів об'єктів.
        public BookProfile()
        {
            //AutoMapper автоматично виконає відображення властивостей одного класу 
            //на властивості іншого класу, якщо їх назви збігаються.

            //Ці конфігурації встановлюють відображення для інших пар типів об'єктів.
            CreateMap<Book, BookReadDto>();
            CreateMap<BookCreateDto, Book>();
            CreateMap<BookReadDto, BookPublishedDto>();
            CreateMap<Book, GrpcBookCollection>()
                .ForMember(dest => dest.BooksId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tittle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner));
        }
    }
}