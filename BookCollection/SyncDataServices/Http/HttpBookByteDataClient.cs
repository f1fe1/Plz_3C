using System.Net.Http;
using System.Text;
using System.Text.Json;
using BookCollection.Dtos;

namespace BookCollection.SyncDataServices.Http
{
    public class HttpBookByteDataClient : IBookByteDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpBookByteDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        //Метод, який відправляє дані на сервер у форматі JSON через HTTP POST-запит.
        public async Task SendBookByteToChapter(BookReadDto plat)
        {
            //Об'єкт, який представляє собою контент, що буде відправлений у тілі запиту
            var httpConetct = new StringContent (
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json");

            //Асинхронний метод для відправки HTTP POST-запиту на вказаний URL.
            var response = await _httpClient.PostAsync($"{_configuration["BookByte"]}" , httpConetct);
            
            //Виведення повідомлення в консоль щодо успішності відправки.
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to BookByte was OK ");
            }

            else
            {
                Console.WriteLine("--> Sync POST to BookByte was NOT OK ");
            }
        }
    }
}