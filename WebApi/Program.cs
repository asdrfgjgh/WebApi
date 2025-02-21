using Microsoft.AspNetCore.Identity;
using WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAuthorization();
//builder.Services
//    .AddIdentityApiEndpoints<IdentityUser>()
//    .AddDapperStores(options =>
//    {
//        options.ConnectionString = dbConnectionString;
//    });
// Add services to the container.
builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);
var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);

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

//app.UseAuthorization();

//app.MapControllers().RequireAuthorization();
//app.MapGroup(prefix: "/account")
//    .MapIdentityApi<IdentityUser>();
//app.MapGet("/", () => "Hello World, the API is up");
//app.MapPost(pattern: "/account/logout",
//    async (SignInManager<IdentityUser> signInManager,
//    [FromBody] object empty) =>
//    {
//        if (empty != null)
//        {
//            await signInManager.SignOutAsync();
//            return Results.Ok();
//        }
//        return Results.Unauthorized();
//    })
//    .RequireAuthorization();




app.Run();
