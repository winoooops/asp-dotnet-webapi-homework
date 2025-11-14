using MyWebAPI.Configs;
using MyWebAPI.Middleware;
using MyWebAPI.Services.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddTransient<RequestLoggingMiddle>();
// configure the 
builder.Services.Configure<DBConfig>(builder.Configuration.GetSection("DBConfig"));


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
