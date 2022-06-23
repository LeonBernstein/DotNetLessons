using DotNetLessons.WebApi.DbModel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DotNetLessonsContext>();

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
