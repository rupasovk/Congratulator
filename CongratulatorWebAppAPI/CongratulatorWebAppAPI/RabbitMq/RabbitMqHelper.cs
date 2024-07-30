using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Configuration;
//using NuGet.Protocol.Plugins;
using System.Text;
using RabbitMQ.Client;
using System.Configuration;

namespace CongratulatorWebAppAPI.RabbitMq
{
    public class RabbitMqHelper
    {
        /// <summary>
        /// Синглтон экземпляр подключения к RabbitMQ
        /// </summary>
        private static ConnectionFactory? _factory;

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
                                                                                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                                                      .Build();

        /// <summary>
        /// Функция создания фабрики подключения к RabbitMQ
        /// </summary>
        /// <param name="configuration"> Конфигурация </param>
        /// <returns></returns>
        private static ConnectionFactory CreateConnectionFactory(IConfiguration configuration)
        {
            var rabbitMQSettings = configuration.GetSection("RabbitMQSettings");

            var HostName = rabbitMQSettings["HostName"];
            var UserName = rabbitMQSettings["UserName"];
            var Password = rabbitMQSettings["Password"];

            return new ConnectionFactory
            {
                HostName = rabbitMQSettings["HostName"],
                UserName = rabbitMQSettings["UserName"],
                Password = rabbitMQSettings["Password"]
            };
        }

        /// <summary>
        /// Отправить сооющение в Rabbit Mq
        /// </summary>
        /// <param name="connection"> Подключение к RabbitMQ</param>
        /// <param name="_message"> Сообщение </param>
        /// <param name="_exchange"> Обменник </param>
        /// <param name="_routingKey"> Ключ </param>
        /// <param name="_basicProperties"> Базовые свойства </param>
        public void SendMessage(IConnection connection, byte[] _message, string _exchange = "", string _routingKey = "", IBasicProperties _basicProperties = null)
        {
            using (var channel = connection.CreateModel())
            {
                var rabbitMQSettings = GetConfiguration().GetSection("RabbitMQSettings");

                channel.BasicPublish(exchange: _exchange != "" ? _exchange : rabbitMQSettings["CongratulationExchange"],
                                     routingKey: _routingKey != "" ? _routingKey : rabbitMQSettings["CongratulationRoutingKey"],
                                     basicProperties: _basicProperties,
                                     body: _message);

                Console.WriteLine($" [x] Sent {Encoding.UTF8.GetString(_message)}");
                Console.WriteLine(" Press [enter] to exit.");
            }
        }
    }
}
