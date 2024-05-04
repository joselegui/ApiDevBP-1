using ApiDevBP.DATA;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ApiDevBP.Repository.IRopository;
using ApiDevBP.Repository;
using ApiDevBP.ApiDevBpMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/Logs-.txt",rollingInterval:RollingInterval.Day)
    .CreateBootstrapLogger();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



//Agregamos los repositorios
#region Repositorios

builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion


builder.Services.AddAutoMapper(typeof(ApiDevBpMapper));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
    });
    var xmlFilename = $"./Documentation.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});


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

app.Run();
