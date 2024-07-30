using Quartz.Impl;
using Quartz;
using CongratulatorWebAppAPI.Jobs;

namespace CongratulatorWebAppAPI.Schedulers
{
    public class CongratulationScheduler
    {
        public static async void Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<CongratulationJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()  // создаем триггер
                .WithIdentity("trigger1", "group1")     // идентифицируем триггер с именем и группой
                .StartNow()                            // запуск сразу после начала выполнения
                .WithSimpleSchedule(x => x            // настраиваем выполнение действия
                    //.WithInterval(TimeSpan.FromSeconds(30))          // через 40 секунд
                    //.WithIntervalInMinutes(1)          // через 1 минуту
                    .WithIntervalInHours(24)          // через 1 день
                    .RepeatForever())                   // бесконечное повторение
                .Build();                               // создаем триггер

            await scheduler.ScheduleJob(job, trigger);        // начинаем выполнение работы
        }
    }
}
