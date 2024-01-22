using AutoMapper;
using BookCollection.AsyncDataServices;
using BookCollection.Data;
using BookCollection.Dtos;
using BookCollection.Models;
using BookCollection.SyncDataServices.Http;
using Microsoft.AspNetCore.Mvc;

//визначає контролер для веб-API в додатку на платформі .NET Core
namespace BookCollection.Controllers
{
    // Вказівка маршруту для всіх методів контролера
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        // Поля контролера
        private readonly IBookRepo _repository; // Репозиторій для роботи з даними про книги
        private readonly IMapper _mapper; // Об'єкт для використання AutoMapper для мапування між об'єктами
        private readonly IBookByteDataClient _bookbyteDataClient;// Клієнт для взаємодії з зовнішнім сервісом BookByteData
        private readonly IMessageBusClient _messageBusClient;// Клієнт для взаємодії з повідомленнями через Message Bus

        // Конструктор контролера, в якому відбувається ініціалізація полів
        public BookController(IBookRepo repository, IMapper mapper , IBookByteDataClient bookbyteDataClient, IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _bookbyteDataClient = bookbyteDataClient; 
            _messageBusClient = messageBusClient;
        }

        // Метод для отримання всіх книг (HTTP GET /api/book)
        [HttpGet]
        public ActionResult<IEnumerable<BookReadDto>> GetBook()
        {
            Console.WriteLine("--> Getting Books....");

            var bookItem = _repository.GetAllBook();

            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(bookItem));
        }
        
         // Метод для отримання книги за ідентифікатором (HTTP GET /api/book/{id})
        [HttpGet("{id}", Name = "GetBookById")]
        public ActionResult<BookReadDto> GetBookById(int id)
        {
            var bookItem = _repository.GetBookById(id);
            if(bookItem != null)
            {
                return Ok(_mapper.Map<BookReadDto>(bookItem));
            }

            return NotFound();
        }

        // Метод для створення нової книги (HTTP POST /api/book)
        public async Task<ActionResult<BookReadDto>> CreateBook(BookCreateDto bookCreateDto)
        {
            var bookModel = _mapper.Map<Book>(bookCreateDto);
            _repository.CreateBook(bookModel);
            _repository.SaveChanges();

            var bookReadDto = _mapper.Map<BookReadDto>(bookModel);

            // Синхронізація з зовнішнім сервісом (Sync)
            try
            {
                await _bookbyteDataClient.SendBookByteToChapter(bookReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously : {ex.Message}");
            }

            // Асинхронна відправка повідомлення через Message Bus (Async)
            try
            {
                var bookPublishedDto = _mapper.Map<BookPublishedDto>(bookReadDto);
                bookPublishedDto.Event = "Book_Published";
                _messageBusClient.PublishNewBook(bookPublishedDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously : {ex.Message}");
            }

            // Повернення відповіді з інформацією про новостворену книгу
            return CreatedAtRoute(nameof(GetBookById), new {id = bookReadDto.Id}, bookReadDto);
        }
        
    }
}

