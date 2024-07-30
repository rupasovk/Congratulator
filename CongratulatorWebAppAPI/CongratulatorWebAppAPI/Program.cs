using CongratulatorWebAppAPI.DataBase;
using CongratulatorWebAppAPI.Jobs;
using CongratulatorWebAppAPI.Schedulers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<CongratulationDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", p => p
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .WithHeaders("content-type")
                .AllowCredentials()
            ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// настраиваем CORS
app.UseCors(builder => builder.AllowAnyOrigin());

// Запуск SMTP Shceduler-а
CongratulationScheduler.Start();

app.Run();
