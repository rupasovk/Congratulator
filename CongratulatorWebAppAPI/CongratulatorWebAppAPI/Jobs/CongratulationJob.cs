using CongratulatorWebAppAPI.Controllers;
using CongratulatorWebAppAPI.DataBase;
using CongratulatorWebAppAPI.RabbitMq;
using CongratulatorWebAppAPI.RabbitMq.MessageObjects;
using Quartz;
using System.Text.Json;

namespace CongratulatorWebAppAPI.Jobs
{
    public class CongratulationJob : IJob
    {
        UsersController userController = new UsersController();
        RabbitMqHelper rabbitMqHelper = new RabbitMqHelper();
        
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Проверка пользователей...", DateTime.Now.ToString("yyyy.MM.dd HH:mm"));

            // Получить пользователей - именинников
            var userList = userController.GetTodayBirthdaysList();

            // Создать экземпляр задания отправки сообщения по протоколу SMTP
            var task = new SmtpMessageTask();

            using (var dbContext = new CongratulationDbContext()) 
            {
                // Получить подключение к сервису RabbitMq
                var connectionFactory = RabbitMqHelper.GetConnectionFactory();

                using (var connection = connectionFactory.CreateConnection())
                {
                    // Для каждого пользователя - именинника
                    foreach (var user in userList)
                    {
                        // Заполнить адрес получателя
                        task.To.Add(user.Email);
                        
                        // Заполнить тему письма
                        task.Subject = "Поздравляем с днем рождения!";

                        // Заполнить тему письма
                        task.Body = string.Format(dbContext.UserCongratulations.OrderBy(x => Guid.NewGuid()).ToList().FirstOrDefault()?.Message ?? "", user.Name, user.SurName) ?? "";

                        // Сериализовать задание на отправку письма
                        var taskJson = JsonSerializer.Serialize(task);

                        // Добавить сообщение в очередь
                        rabbitMqHelper.SendMessage(connection, System.Text.Encoding.UTF8.GetBytes(taskJson ?? ""));
                    }
                }
            }
        }

        public void ExecuteSyncTest()
        {
            Console.WriteLine("Проверка пользователей...", DateTime.Now.ToString("yyyy.MM.dd HH:mm"));

            // Получить пользователей - именинников
            var userList = userController.GetTodayBirthdaysList();

            // Создать экземпляр задания отправки сообщения по протоколу SMTP
            var task = new SmtpMessageTask();

            using (var dbContext = new CongratulationDbContext())
            {
                // Получить подключение к сервису RabbitMq
                var connectionFactory = RabbitMqHelper.GetConnectionFactory();

                using (var connection = connectionFactory.CreateConnection())
                {
                    // Для каждого пользователя - именинника
                    foreach (var user in userList)
                    {
                        // Заполнить адрес получателя
                        task.To.Add(user.Email);

                        // Заполнить тему письма
                        task.Subject = "Поздравляем с днем рождения!";

                        // Заполнить тему письма
                        task.Body = $"Уважаемый {user.Name} {user.SurName}\n" + dbContext.UserCongratulations.OrderBy(x => Guid.NewGuid()).ToList().FirstOrDefault()?.Message ?? "";

                        // Сериализовать задание на отправку письма
                        var taskJson = JsonSerializer.Serialize(task);

                        // Добавить сообщение в очередь
                        rabbitMqHelper.SendMessage(connection, System.Text.Encoding.UTF8.GetBytes(taskJson ?? ""));
                    }
                }
            }
        }
    }
}
