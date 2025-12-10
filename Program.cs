using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyWebAPI.Configs;
using MyWebAPI.Middleware;
using MyWebAPI.Models.Context;
using MyWebAPI.Services;
using MyWebAPI.Services.Auth;
using MyWebAPI.Services.Teacher;
using MyWebAPI.Services.User;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<DBConfig>(builder.Configuration.GetSection("DBConfig"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));
builder.Services.AddDbContext<ModelDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserServiceEF, UserServiceEF>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IIamUserService, IamUserService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<RequestLoggingMiddle>();

var jwtSection =  builder.Configuration.GetSection("JWT");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidIssuer = jwtSection["Issuer"],
      ValidateAudience = false,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(key),
      ValidateLifetime = true,
      ClockSkew = TimeSpan.FromMinutes(1),
      RoleClaimType = ClaimTypes.Role
    };
  });
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
  {
    Title = "My First Web Api Project",
    Version = "v1",
    Description = "This is my First Web Api Project",
    Contact = new Microsoft.OpenApi.Models.OpenApiContact() { Name = "will", Url = new Uri("https://google.com") }
  });

  var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
  if (File.Exists(xmlPath))
  {
    options.IncludeXmlComments(xmlPath, true);
  }

  options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
  {
    Description = "",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
  });

  options.AddSecurityRequirement(new OpenApiSecurityRequirement()
  {
    {

      new OpenApiSecurityScheme()
      {
        Reference=new OpenApiReference()
        {
          Type=ReferenceType.SecurityScheme,
          Id="Bearer"
        }
      },
      new List<string>()
    }
  });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRequestLogging();
app.MapControllers();

app.Run();
