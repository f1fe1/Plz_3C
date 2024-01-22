using AutoMapper;
using BookByte.Dtos;
using BookByte.Models;
using BookCollection;


namespace BookByte.Profiles
{
    //Цей клас наслідується від класу Profile, який надається AutoMapper для визначення профілю мапінгу.
    public class ChapterProfiles : Profile
    {
        //Це конструктор класу ChapterProfiles, в якому визначаються правила мапінгу.
        public ChapterProfiles()
        {
            //Цей метод вказує AutoMapper використовувати мапінг для перетворення об'єктів типу BookBytes в об'єкти типу BookByteReadDto.
            CreateMap<BookBytes, BookByteReadDto>();
            //Тут вказується мапінг для об'єктів типу ChapterCreateDto у об'єкти типу Chapter.
            CreateMap<ChapterCreateDto, Chapter>();
            //цей метод вказує AutoMapper мапити об'єкти типу Chapter на об'єкти типу ChapterReadDto
            CreateMap<Chapter, ChapterReadDto>();
            //ут визначаються додаткові правила мапінгу для об'єктів типу BookBytesPublishedDto та GrpcBookCollection у об'єкти типу BookBytes.
            CreateMap<BookBytesPublishedDto, BookBytes>()
            //ForMember ---> Цей метод дозволяє вказати які поля об'єкта призначеного для мапінгу слід заповнити тими даними, які знаходяться в джерелі
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcBookCollection, BookBytes>()
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.BooksId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Tittle))
                .ForMember(dest => dest.Chapters, opt => opt.Ignore());
        }
    }
}