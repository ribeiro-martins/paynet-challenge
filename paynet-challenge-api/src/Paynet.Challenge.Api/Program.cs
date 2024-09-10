using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Paynet.Challenge.Core.Services.Auth;
using Paynet.Challenge.Core.Services.Cep;
using Paynet.Challenge.Core.Services.Db;
using Paynet.Challenge.Core.Services.Email;
using Paynet.Challenge.Core.Services.User;
using Paynet.Challenge.Core.Settings;
using Paynet.Challenge.Repository;
using Paynet.Challenge.Repository.User;

var builder = WebApplication.CreateBuilder(args);
var settings = new AppSettings();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.UseSecurityTokenValidators = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigninKey)),
        ValidIssuer = settings.Issuer,
        ValidAudience = settings.Audience,
        ClockSkew = TimeSpan.Zero
    };
});

MongoClient client = new MongoClient(settings.ConnectionString);
builder.Services.AddSingleton<ISettings>(settings);
builder.Services.AddSingleton<IUserRepository>(new UserRepository(client.GetDatabase("paynet")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient<ICepService, CepService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();
 app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var userService = app.Services.GetService<IUserService>();
var dbInizializer = new DbInizializer(userService);
dbInizializer.Init();
app.Run();