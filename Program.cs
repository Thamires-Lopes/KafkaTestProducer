
using KafkaTestProducer.Models;
using KafkaTestProducer.Services;
using Microsoft.AspNetCore.Mvc;

namespace KafkaTestProducer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<KafkaProducerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapPost("/sendMessageProducer", async ([FromServices] KafkaProducerService service, [FromBody] User user) =>
            {
                return await service.SendMessage(user);
            });


            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            app.Run();
        }
    }
}
