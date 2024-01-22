using AutoMapper;
using BookByte.Data;
using BookByte.Dtos;
using BookByte.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookByte.Controllers
{
    // вказує, що URL для цього контролера буде містити "api/c/bookbytes/{bookbyteId}/chapter".
    [Route("api/c/bookbytes/{bookbyteId}/[controller]")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly IBookByteRepo _repository;
        private readonly IMapper _mapper;

        //Контролер приймає репозиторій (IBookByteRepo) та AutoMapper (IMapper) через конструктор
        public ChapterController(IBookByteRepo repository , IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]//Обробка GET-запитів для отримання розділів книги.
        //Використовує AutoMapper для мапінгу глав на ChapterReadDto.
        public ActionResult<IEnumerable<ChapterReadDto>> GetChapterForBookByte(int bookbyteId)
        {
            Console.WriteLine($"--> Hit GetChapterForBookByte: {bookbyteId}");
            //Перевіряє, чи існує об'єкт BookByte за заданим bookbyteId.
            if(_repository.BookByteExits(bookbyteId))
            {
                return NotFound();
            }

            //Викликає метод _repository.GetChapterForBookByte для отримання глав.
            var chapter = _repository.GetChapterForBookByte(bookbyteId);
            //Повертає результат у форматі JSON.
            return Ok(_mapper.Map<IEnumerable<ChapterReadDto>>(chapter));
            
        }

        [HttpGet("{chapterId}" , Name = "GetChaptersForBookByte")]
        //Використовує AutoMapper для мапінгу глави на ChapterReadDto.
        public ActionResult<ChapterReadDto> GetChaptersForBookByte(int bookbyteId, int chapterId)
        {
            Console.WriteLine($"--> Hit GetChapterForBookByte: {bookbyteId} / {chapterId}");
            
            //Перевіряє, чи існує об'єкт BookByte та глава за вказаними bookbyteId та chapterId.
            if(_repository.BookByteExits(bookbyteId))
            {
                return NotFound();
            }

            //Викликає метод _repository.GetChapter для отримання конкретної глави.
            var chapters = _repository.GetChapter(bookbyteId, chapterId);

            if(chapters == null)
            {
                return NotFound();
            }
            //Повертає результат у форматі JSON.
            return Ok(_mapper.Map<IEnumerable<ChapterReadDto>>(chapters));
        }

        [HttpPost]
        public ActionResult<ChapterReadDto> CreateChapterForBookByte(int bookbyteId, ChapterCreateDto chapterDto)
        {
            Console.WriteLine($"--> Hit CreateChapterForBookByte: {bookbyteId}");

            //Перевіряє, чи існує об'єкт BookByte за заданим bookbyteId.
            if(_repository.BookByteExits(bookbyteId))
            {
                return NotFound();
            }

            //Викликає метод _mapper.Map для створення об'єкту Chapter з об'єкту ChapterCreateDto.
            var chapter = _mapper.Map<Chapter>(chapterDto);
            //Викликає метод _repository.CreateChapter для збереження нової глави.
            _repository.CreateChapter(bookbyteId, chapter);
            //Викликає _repository.SaveChanges() для збереження змін в базі даних.
            _repository.SaveChanges();

            //Викликає _mapper.Map для мапінгу створеної глави на ChapterReadDto.
            var chapterReadDto = _mapper.Map<ChapterReadDto>(chapter);

            //Повертає відповідь з кодом 201 (Created) та посиланням на новостворену главу.
            return CreatedAtRoute(nameof(GetChaptersForBookByte), 
                new {bookbyteId = bookbyteId, chapterId = chapterReadDto.Id}, chapterReadDto);
        }
    }
}