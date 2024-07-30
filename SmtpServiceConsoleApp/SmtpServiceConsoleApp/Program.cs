using SmtpServiceConsoleApp.Jobs;
using SmtpServiceConsoleApp.RabbitMq.MessageObjects;
using SmtpServiceConsoleApp.Schedulers;
using SmtpServiceConsoleApp.Services;
try
{
    Console.WriteLine("Hello, World!");

    //SmtpHelper smtpHelper = new SmtpHelper();

    //SmtpMessageTask smtpMessageTask = new SmtpMessageTask()
    //{
    //    Message = "Test",
    //    Subject = "Test subject",
    //    To = new List<string>() { "kattoshkin@gmail.com" }
    //};

    //var smtpMessage = smtpHelper.GetMessage(smtpMessageTask);
    //var smtpClient = smtpHelper.GetSmtpClient();

    //smtpHelper.SendMessage(smtpClient, smtpMessage);
    new RabbitMqConsumerJob().ExecuteSync();

    //RabbitMqConsumerScheduler.Start();

    Console.ReadLine();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"SERVICE EXCEPTION! {ex.Message}");
    Console.ResetColor();
}