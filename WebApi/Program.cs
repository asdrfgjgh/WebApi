using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});
    //.AddIdentityApiEndpoints<IdentityUser>(options =>
    //{
    //    options.Password.RequireDigit = true;
    //    options.Password.RequiredLength = 8;
    //    options.Password.RequireNonAlphanumeric = false;
    //    options.Password.RequireUppercase = true;
    //    options.Password.RequireLowercase = false;
    //    options.User.RequireUniqueEmail = true;
    //})
    //.AddDapperStores(options =>
    //{
        //var sqlConnectionString = Environment.GetEnvironmentVariable("SQLConnectionString");
        //if (string.IsNullOrWhiteSpace(sqlConnectionString))
        //{
        //    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");
        //}
    //    options.ConnectionString = sqlConnectionString;
    //});
//// Add services to the container.
//builder.Services.Configure<BearerTokenOptions>(options =>
//{
//    options.TokenValidityInMinutes = 60;
//    options.Issuer = "YourIssuer";
//    options.Audience = "YourAudience";
//});
builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);
var sqlConnectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

Console.WriteLine($"SQL Connection String: {sqlConnectionString}");

var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);
if (string.IsNullOrWhiteSpace(sqlConnectionString))
{
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");
}




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

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();//.RequireAuthorization();
//app.MapGroup("/account")
//    .MapIdentityApi<IdentityUser>();

//app.MapPost("/account/logout", async (SignInManager<IdentityUser> signInManager) =>
//{
//                await signInManager.SignOutAsync();
//                return Results.Ok();
//}).RequireAuthorization();

app.MapGet("/", () => "Hello World, the API is up");



app.Run();
