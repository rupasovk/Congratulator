using Microsoft.Extensions.Configuration;
using SmtpServiceConsoleApp.RabbitMq.MessageObjects;
using System.Net;
using System.Net.Mail;

namespace SmtpServiceConsoleApp.Services
{
    internal class SmtpHelper
    {
        string smtpServer = "";
        int smtpPort = 587;
        string smtpUsername = "";
        string smtpPassword = "";
        string from = "";

        IConfiguration configuration;

        public SmtpHelper()
        {
            configuration = GetConfiguration();

            // Настройка параметров SMTP-сервера
            var smtpSettings = configuration.GetSection("SmtpClient");

            smtpServer = smtpSettings["SmtpServer"] ?? "Unknown";
            smtpPort = int.Parse(smtpSettings["SmtpPort"] ?? "0");

            smtpUsername = smtpSettings["SmtpUsername"] ?? "Unknown";
            smtpPassword = smtpSettings["SmtpPassword"] ?? "Unknown";
            from = smtpSettings["From"] ?? "Unknown";
        }

        /// <summary>
        /// Получить конфигурацию SMTP
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetConfiguration() => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                                      .AddJsonFile("appconfig.json", optional: false, reloadOnChange: true)
                                                                                      .Build();

        public MailMessage GetMessage(SmtpMessageTask task)
        {
            // Создание объекта MailMessage
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            foreach(var address in task.To)
            {
                message.To.Add(new MailAddress(address));
            }
            message.Subject = task.Subject;
            message.Body = task.Body;

            return message;
        }

        public SmtpClient GetSmtpClient()
        {
            // Создание клиента SMTP
            SmtpClient client = new SmtpClient(smtpServer, smtpPort);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            client.EnableSsl = true;

            return client;
        }

        public void SendMessage(SmtpClient client, MailMessage message)
        {
            try
            {
                // Отправка сообщения
                client.Send(message);
                Console.WriteLine("Сообщение успешно отправлено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при отправке сообщения: " + ex.Message);
            }
        }
    }
}
