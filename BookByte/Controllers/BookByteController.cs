using AutoMapper;
using BookByte.Data;
using BookByte.Dtos;
using Microsoft.AspNetCore.Mvc;

// для створення веб-додатків. 
namespace BookByte.Controllers
{   

    //Атрибут маршрутизації, який вказує URL-шлях для доступу до методів цього контролера.
    [Route("api/c/[controller]")] //Атрибут, що позначає клас як контролер для обробки HTTP-запитів.
    [ApiController]
    
    
    public class BookByteController : ControllerBase
    {
        private readonly IBookByteRepo _repository;
        private readonly IMapper _mapper;

        //Ці параметри використовуються для взаємодії з репозиторієм та AutoMapper.
        public BookByteController(IBookByteRepo repository, IMapper mapper )
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        [HttpGet] //Атрибут, що позначає метод як обробник HTTP GET-запитів.
        //Метод повертає перелік книг у форматі BookByteReadDto за допомогою репозиторію та мапера AutoMapper.
        public ActionResult<IEnumerable<BookByteReadDto>> GetBookBytes()
        {
            Console.WriteLine("--> Getting BookBytes from BookByte");
            
            var bookbyteItems = _repository.GetAllBookByte();

            return Ok(_mapper.Map<IEnumerable<BookByteReadDto>>(bookbyteItems));
        }

        [HttpPost]//Атрибут, що позначає метод як обробник HTTP POST-запитів.
        //Метод призначений для тестування вхідного з'єднання і повертає рядок у відповідь.
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test of from Platforms Controler");
        }
        
    }
}