using Quartz.Impl;
using Quartz;
using SmtpServiceConsoleApp.Jobs;

namespace SmtpServiceConsoleApp.Schedulers
{
    internal class RabbitMqConsumerScheduler
    {
        public static async void Start()
        {
            Console.WriteLine("RabbitMqJob is starting...");

            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<RabbitMqConsumerJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger1", "group1")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    .WithInterval(TimeSpan.FromSeconds(40))          // через 1 sec
                    //.WithIntervalInMinutes(1)          // через 1 sec
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер

            await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
