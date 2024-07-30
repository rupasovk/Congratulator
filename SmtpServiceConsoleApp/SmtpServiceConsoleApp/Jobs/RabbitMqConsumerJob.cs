using Quartz;
using SmtpServiceConsoleApp.RabbitMq;
using SmtpServiceConsoleApp.RabbitMq.MessageObjects;
using SmtpServiceConsoleApp.Services;
using System.Text.Json;

namespace SmtpServiceConsoleApp.Jobs
{
    internal class RabbitMqConsumerJob : IJob
    {
        SmtpHelper smtpHelper = new SmtpHelper();

        public Task Execute(IJobExecutionContext context)
        {
            var factory = RabbitMqHelper.GetConnectionFactory();

            var message = "";

            using (var connection = factory.CreateConnection())
            {
                message = RabbitMqHelper.GetMessageString(connection);
            }

            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine($"No messages in Queue");
                return Task.CompletedTask;
            }
            Console.WriteLine($"The message is: \n{message}");

            SmtpMessageTask? smtpMessageTask = JsonSerializer.Deserialize<SmtpMessageTask>(message);
             
            var smtpMessage = smtpHelper.GetMessage(smtpMessageTask);
            var smtpClient = smtpHelper.GetSmtpClient();

            smtpHelper.SendMessage(smtpClient, smtpMessage);

            return Task.CompletedTask;
        }

        // TEST
        public void ExecuteSync()
        {
            var factory = RabbitMqHelper.GetConnectionFactory();

            var message = "";

            using (var connection = factory.CreateConnection())
            {
                message = RabbitMqHelper.GetMessageString(connection);
            }

            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine($"No messages in Queue");
                return;
            }
            Console.WriteLine($"The message is: \n{message}");

            SmtpMessageTask? smtpMessageTask = JsonSerializer.Deserialize<SmtpMessageTask>(message);
             
            var smtpMessage = smtpHelper.GetMessage(smtpMessageTask);
            var smtpClient = smtpHelper.GetSmtpClient();

            smtpHelper.SendMessage(smtpClient, smtpMessage);
        }
    }
}
