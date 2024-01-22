using AutoMapper;
using BookByte.Models;
using BookCollection;
using Grpc.Net.Client;
using BookByte.Data;
using BookByte.Dtos;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;


namespace BookByte.SyncDataServices
{
    //Цей клас є реалізацією інтерфейсу IBookByteData
    public class BookByteData : IBookByteData
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        //Клас приймає в конструкторі два параметри: об'єкт конфігурації (IConfiguration) і об'єкт IMapper.
        public BookByteData(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        //Метод викликає GRPC-сервіс для отримання всіх книг.
        public IEnumerable<BookBytes> ReturnAllBookByte()
        {
            //Створюється канал і клієнт GRPC для виклику сервісу.
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcBook"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcBook"]);
            var clients = new GrpcBook.GrpcBookClient(channel);
            var request = new GetAllRequest();//Формується запит GetAllRequest.

            //Логується спроба виклику GRPC-сервісу за допомогою конфігураційного параметра GrpcBook.
            try
            {
                //Здійснюється виклик GRPC-сервісу, і результат мапиться на тип IEnumerable<BookBytes> за допомогою AutoMapper.
                var reply = clients.GetAllBookByte(request);
                return _mapper.Map<IEnumerable<BookBytes>>((IEnumerable<object>)reply.GetType().GetProperty("BookBytes").GetValue(reply));


            }
            catch(Exception ex)
            {
                //У разі виникнення винятку (Exception), логується повідомлення про невдалу спробу та повертається null.
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}