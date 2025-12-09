using Microsoft.EntityFrameworkCore;
using MyWebAPI.Configs;
using MyWebAPI.Middleware;
using MyWebAPI.Models.Context;
using MyWebAPI.Services;
using MyWebAPI.Services.Teacher;
using MyWebAPI.Services.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<DBConfig>(builder.Configuration.GetSection("DBConfig"));
builder.Services.AddDbContext<ModelDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<IUserServiceEF, UserServiceEF>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddTransient<RequestLoggingMiddle>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseRequestLogging();

app.Run();
