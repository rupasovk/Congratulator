using CongratulatorWebAppAPI.RabbitMq;

namespace CongratulatorWebAppAPI.Controllers
{
    public class RabbitMqController
    {
        public void ProduceMessage(string exchange, string routingKey)
        {
            var _isAlive = true;

            var factory = RabbitMqHelper.GetConnectionFactory();
            RabbitMqHelper rabbitMqHelper = new RabbitMqHelper();

            //Console.WriteLine("Write the exchange");
            //var exchange = Console.ReadLine();

            //Console.WriteLine("Write the reutingKey");
            //var routingKey = Console.ReadLine();

            using (var connection = factory.CreateConnection())
            {
                do
                {
                    Console.WriteLine("Write the message");
                    var message = Console.ReadLine();

                    if (!string.IsNullOrEmpty(message))
                    {
                        rabbitMqHelper.SendMessage(connection, System.Text.Encoding.UTF8.GetBytes(message ?? ""), exchange ?? "", routingKey ?? "");

                        Console.WriteLine($"The message: {message}\nSuccessfuly sended!");
                    }
                    else
                    {
                        _isAlive = false;
                    }
                } while (_isAlive);
            }

        }
    }
}
