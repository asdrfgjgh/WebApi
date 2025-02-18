using WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);
var sqlConnectionString = builder.Configuration["SqlConnectionString"];

if (string.IsNullOrWhiteSpace(sqlConnectionString))
throw new InvalidProgramException("Configuration variable SqlConnectionString not found");
builder.Services.AddTransient<IRepository, Repository>(o => new Repository(sqlConnectionString));

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

app.MapControllers();

app.Run();
