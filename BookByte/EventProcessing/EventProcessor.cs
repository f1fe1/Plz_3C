using System.Text.Json;
using AutoMapper;
using BookByte.Data;
using BookByte.Dtos;
using BookByte.Models;
using Microsoft.Extensions.DependencyInjection;

namespace BookByte.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        //Конструктор приймає два параметри: IServiceScopeFactory і AutoMapper.IMapper.
        public EventProcessor(IServiceScopeFactory scopeFactory,AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        //Метод отримує рядок message, який представляє інформацію про подію.
        public void ProcessEvent(string message)
        {
            //Викликає метод DetermineEvent для визначення типу події.
            var eventType = DetermineEvent(message);

            //В залежності від типу події викликає відповідний метод обробки.
            switch(eventType)
            {
                case EventType.BookBytesPublished:
                    addBookBytes(message);
                    break;
                default:
                    break;
            }
        }

        //Метод визначає тип події, розгортаючи рядок notifcationMessage за допомогою JSON-серіалізації у GenericEventDto.
        private EventType DetermineEvent(string  notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage); 

            //Якщо тип події є "Platform_Published", повертає EventType.BookBytesPublished, інакше повертає EventType.Undermined.
            switch(eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.BookBytesPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undermined;
            }
        }

        //Метод обробляє подію типу BookBytesPublished.
        private void addBookBytes(string BookBytesPublishedMessage)
        {
            //Спочатку відбувається створення області служб (scope),
            // під час якої викликається метод для роботи з базою даних.
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBookByteRepo>();
                
                //Перетворює рядок BookBytesPublishedMessage у об'єкт BookBytesPublishedDto
                var BookBytesPublishedDto = JsonSerializer.Deserialize<BookBytesPublishedDto>(BookBytesPublishedMessage);

                //Використовує AutoMapper для відображення BookBytesPublishedDto на об'єкт BookBytes.
                try
                {
                    //Перевіряє, чи не існує вже запис з таким ISBN у базі даних.
                    var plat = _mapper.Map<BookBytes>(BookBytesPublishedDto);
                    //Якщо не існує, створює новий запис та зберігає його в базі даних.
                    if(!repo.ExternalBookByteExist(plat.ISBN))
                    {
                        repo.CreateBookByte(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> Platform added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Platform already exisits...");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
                }
            }    
        }
    }

    //Просте перерахування, яке містить можливі типи подій.
    enum EventType
    {
        BookBytesPublished,
        Undermined
    }
}