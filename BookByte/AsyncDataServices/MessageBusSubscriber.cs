
using System.Text;
using BookByte.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BookByte.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        // Поля класу
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        // Конструктор класу
        public MessageBusSubscriber(IConfiguration configuration , IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;    

             // Ініціалізація RabbitMQ
            InitializeRabbitMQ();
        }

         // Ініціалізація RabbitMQ
        private void InitializeRabbitMQ()
        {
            // Створення фабрики підключення RabbitMQ
            var factory = new ConnectionFactory() {HostName = _configuration["RabbitMQHost"], Port = int.Parse(_configuration["RabbitMQPort"])};

            // Створення підключення та каналу
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Оголошення обмінника та черги
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName,
                exchange: "trigger",
                routingKey: "");

            Console.WriteLine("--> Listening on the Message Bus...");

            // Додавання обробників подій
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }
        
         // Перевизначений метод з BackgroundService для виконання асинхронної роботи
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            
            // Створення споживача повідомлень
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Recived!");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
            };

            // Початок споживання повідомлень
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        // Обробник події відключення RabbitMQ
        private void RabbitMQ_ConnectionShutdown(object sender , ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connecting shutdown");
        }

        // Перевизначений метод для вивільнення ресурсів
        public override void Dispose()
        {
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }

    }
}