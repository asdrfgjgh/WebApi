using Microsoft.AspNetCore.Identity;
using WebApi;
using WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

//identity middleware = builder.Build();

var sqlConnectionString = builder.Configuration["SQLConnectionString"];
//var sqlConnectionString = builder.Configuration.GetValue<string>("SqlConnectionString");
var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);


if (string.IsNullOrWhiteSpace(sqlConnectionString))
    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");

// Add services to the container.
builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 50;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = sqlConnectionString;
});


//builder.Services
//    .AddOptions<BearerTokenOptions>(IdentityConstants.BearerScheme)
//    .Configure(options =>
//    {
//        options.BearerTokenExpiration = TimeSpan.FromMinutes(60);
//   });


// Adding the HTTP Context accessor to be injected. This is needed by the AspNetIdentityUserRepository
// to resolve the current user.
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();
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

app.MapGroup("/account")
      .MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();

app.MapGet("/", () => $"The API is up . Connection string found: {(sqlConnectionStringFound ? "good" : "bad")}");

app.UseAuthorization();

//app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();
//using Microsoft.AspNetCore.Identity;
//using WebApi.Repositories;


//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAuthorization();

//builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);
//builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
//{
//    options.User.RequireUniqueEmail = true;
//    options.Password.RequiredLength = 50;
//})
//.AddRoles<IdentityRole>()
//.AddDapperStores(options =>
//{
//    var sqlConnectionString = builder.Configuration("SQLConnectionString");
//        if (string.IsNullOrWhiteSpace(sqlConnectionString))
//    {
//        throw new InvalidProgramException("Configuration variable SqlConnectionString not found");
//    }
//    options.ConnectionString = sqlConnectionString;
//});

//builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

//var sqlConnectionString = Environment.GetEnvironmentVariable("SQLConnectionString");

//Console.WriteLine($"SQL Connection String: {sqlConnectionString}");

////var sqlConnectionStringFound = !string.IsNullOrWhiteSpace(sqlConnectionString);
//if (string.IsNullOrWhiteSpace(sqlConnectionString))
//{
//    throw new InvalidProgramException("Configuration variable SqlConnectionString not found");
//}


//builder.Services.AddTransient<IRepository, Repository>(o => new Repository(sqlConnectionString));
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
////builder.Services.AddHttpContextAccessor();
//builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();


//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers().RequireAuthorization();
//app.MapGroup("/account")
//    .MapIdentityApi<IdentityUser>();

//app.MapPost("/account/logout", async (SignInManager<IdentityUser> signInManager) =>
//{
//await signInManager.SignOutAsync();
//return Results.Ok();
//}).RequireAuthorization();

//app.MapGet("/", () => "Hello World, the API is up");



//app.Run();
