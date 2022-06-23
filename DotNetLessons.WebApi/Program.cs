using DotNetLessons.WebApi.DbModel;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
ThreadPool.SetMaxThreads(Environment.ProcessorCount, completionPortThreads);
ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);

ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
ThreadPool.SetMaxThreads(Environment.ProcessorCount, minCompletionPortThreads);
ThreadPool.GetMaxThreads(out minWorkerThreads, out minCompletionPortThreads);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<DotNetLessonsContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DotNetLessonsContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.DisplayRequestDuration());
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(config => config.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
