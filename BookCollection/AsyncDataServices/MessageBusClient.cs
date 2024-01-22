using System.Text;
using System.Text.Json;
using BookCollection.Dtos;
using RabbitMQ.Client;

namespace BookCollection.AsyncDataServices
{
    //реалізує інтерфейс IMessageBusClient.
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        //Конструктор класу приймає об'єкт конфігурації (IConfiguration), 
        //ініціалізує підключення до RabbitMQ і створює канал для обміну повідомленнями.
        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() {HostName = _configuration["RabbitMQHost"], 
            Port = int.Parse(_configuration["RabbitMQPort"])};
            
            //У випадку успішного підключення, виводиться повідомлення про з'єднання
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger" , type : ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to MessageBus");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could Not connect to the Message Bus: {ex.Message}");
            }
        }

        //Призначений для публікації нового об'єкта BookPublishedDto у чергу RabbitMQ.
        //Серіалізує об'єкт в JSON-рядок і викликає метод SendMessage для відправлення повідомлення.
        public void PublishNewBook(BookPublishedDto bookPublishedDto)
        {
            var message = JsonSerializer.Serialize(bookPublishedDto);

            if(_connection.IsOpen)
            {
                Console.WriteLine("RabbitMQ Connection Open, sending message....");
                SendMessage(message);
            } 
            else
            {
                Console.WriteLine("RabbitMQ Connection closed, not sending ");
            }
        }

        //Відправляє серіалізоване повідомлення в обмінник (exchange) з ім'ям "trigger" (тип обмінника - Fanout).
        //Виводить повідомлення про відправлення.
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                                routingKey: "",
                                basicProperties: null,
                                body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        //Реалізація інтерфейсу IDisposable, дозволяє коректно вивільнити ресурси при завершенні роботи з об'єктом.
        //Закриває канал і з'єднання з RabbitMQ.
        public void Dispose()
        {
            Console.WriteLine("MessageBus Dispose");
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        
        //Виводить повідомлення при зупинці з'єднання з RabbitMQ.
        private void RabbitMQ_ConnectionShutdown(object sender , ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}