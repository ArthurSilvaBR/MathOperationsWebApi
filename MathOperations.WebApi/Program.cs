
using MathOperations.Services;
using MathOperations.Services.Interfaces;
using MathOperations.WebApi.Exception;
using Microsoft.Extensions.Logging;

namespace MathOperations.WebApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      
      builder.Logging.ClearProviders();
      builder.Logging.AddConsole();

      // Add services to the container.
      builder.Services.AddScoped<IMathExpressionEvualationService, MathExpressionEvualationService>();

      builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
      builder.Services.AddProblemDetails();

      builder.Services.AddLogging();

      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseAuthorization();
      app.UseExceptionHandler();


      app.MapControllers();

      app.Run();
    }
  }
}
