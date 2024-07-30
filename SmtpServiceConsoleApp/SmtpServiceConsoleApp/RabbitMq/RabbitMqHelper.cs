using System.Text;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SmtpServiceConsoleApp.RabbitMq
{
    internal class RabbitMqHelper
    {
        /// <summary>
        /// Синглтон экземпляр подключения к RabbitMQ
        /// </summary>
        private static ConnectionFactory? _factory;

        public static string Message = "";

        /// <summary>
        /// Получить экземпляр подключения к RabbitMQ
        /// </summary>
        public static ConnectionFactory GetConnectionFactory()
        {
            if (_factory == null)
            {
                var configuration = GetConfiguration();
                _factory = CreateConnectionFactory(configuration);
            }

            return _factory;
        }

        /// <summary>
        /// Получить конфигурацию RabbitMQ
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetConfiguration() => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                                      .AddJsonFile("appconfig.json", optional: false, reloadOnChange: true)
                                                                                      .Build();

        /// <summary>
        /// Функция создания фабрики подключения к RabbitMQ
        /// </summary>
        /// <param name="configuration"> Конфигурация </param>
        /// <returns></returns>
        private static ConnectionFactory CreateConnectionFactory(IConfiguration configuration)
        {
            var rabbitMQSettings = configuration.GetSection("RabbitMQSettings");

            return new ConnectionFactory
            {
                HostName = rabbitMQSettings["HostName"],
                UserName = rabbitMQSettings["UserName"],
                Password = rabbitMQSettings["Password"]
            };
        }


        public static (byte[], string) GetMessage(IConnection connection)
        {
            var configuration = GetConfiguration();
            var rabbitMQSettings = configuration.GetSection("RabbitMQSettings");

            byte[] _body = null;
            string message = "";

            using (var channel = connection.CreateModel())
            {
                // Получаем одно сообщение из очереди
                var deliverResult = channel.BasicGet(rabbitMQSettings["CongratulationQueue"], autoAck: false);

                if (deliverResult != null)
                {
                    _body = deliverResult.Body.ToArray();
                    message = Encoding.UTF8.GetString(_body);

                    // Подтверждаем получение сообщения
                    channel.BasicAck(deliverResult.DeliveryTag, false);
                }
            }

            return (_body, message);
        }

        public static (byte[], string) GetMessage1(IConnection connection)
        {
            var configuration = GetConfiguration();

            var rabbitMQSettings = configuration.GetSection("RabbitMQSettings");

            var _body = new byte[] { };
            var message = "";

            var messageReceived = new ManualResetEventSlim(false);

            using (var channel = connection.CreateModel())
            {
                // Создаем потребителя для получения сообщений
                var consumer = new EventingBasicConsumer(channel);
                // Обработчик события Received
                consumer.Received += (model, ea) =>
                {
                    _body = ea.Body.ToArray();
                    message = Encoding.UTF8.GetString(_body);
                    //Console.WriteLine($"Received message: {message}");

                    // Обработка сообщения
                    // ...

                    // Подтверждаем получение сообщения, только если канал не закрыт
                    if (!channel.IsClosed)
                    {
                        // Подтверждаем получение сообщения
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    }

                    // Сигнализируем о получении сообщения
                    messageReceived.Set();
                };

                // Начинаем получение сообщений из очереди
                channel.BasicConsume(
                    queue: rabbitMQSettings["CongratulationQueue"],
                    autoAck: false,
                    consumer: consumer);

                // Ждем, пока сообщение будет получено
                messageReceived.Wait();
            }
            return (_body, message);
        }

        public static string GetMessageString(IConnection connection)
        {
            //var body = .ToArray();Encoding.UTF8.GetString(
            var body = GetMessage(connection);

            return body.Item2;
        }

        public async Task<byte[]> GetMessageAsync(IConnection connection)
        {
            byte[] body = null;

            using (var channel = connection.CreateModel())
            {
                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    body = ea.Body.ToArray();
                    //await channel.BasicAckAsync(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queue: "q1", autoAck: false, consumer: consumer);

                // Ожидаем получения сообщения
                await Task.Delay(TimeSpan.FromSeconds(10));
            }

            return body;
        }
    }
}
