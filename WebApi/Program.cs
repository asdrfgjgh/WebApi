
using Microsoft.AspNetCore.Identity;
using WebApi.Repositories2D;
using WebApi.Repositories;


var builder = WebApplication.CreateBuilder(args);

//identity middleware = builder.Build();

var sqlConnectionString = builder.Configuration["SqlConnectionString"];
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
    options.Password.RequiredLength = 10;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
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
builder.Services.AddTransient<IObject2DRepository, Object2DRepository>(o => new Object2DRepository(sqlConnectionString));
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

app.MapGet("/", () => $"The API is up . Connection string found: {(sqlConnectionStringFound ? "very good" : "very bad")}");

app.UseAuthorization();

//app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();