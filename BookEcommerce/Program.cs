using BookEcommerce.Models.DAL;
using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BookEcommerce.Services.Interfaces;
using BookEcommerce.Services;
using System.Text;
using BookEcommerce.Services.Mapper;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DAL.Repositories;
using AutoMapper;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(options => options.AddProfile(typeof(MapperProfile)));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly("BookEcommerce"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
        RequireExpirationTime = true,
    };
});

//builder.Services.Configure<MailSettings>();

//DB
builder.Services
    .AddScoped((Func<IServiceProvider, Func<ApplicationDbContext>>)((provider) => () => provider.GetService<ApplicationDbContext>()!))
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<DbFactory>();


//root service
builder.Services
    .AddScoped<MailSettings>()
    .AddScoped<MimeMessage>()
    .AddScoped<SmtpClient>();

//service
builder.Services
    .AddScoped<IVerifyAccountService, VerifyAccountService>()
    .AddScoped<IAuthenticationService, AuthenticationService>()
    .AddScoped<ITokenService, TokenService>()
    .AddScoped<ICustomerService, CustomerService>();


//repo
builder.Services
    .AddScoped<IRoleRepository, RoleRepository>()
    //.AddScoped<Profile, MapperProfile>()
    .AddScoped<IMapper, Mapper>()
    .AddScoped<ISendMailRepository, SendMailRepository>()
    .AddScoped<IAuthenticationRepository, AuthenticationRepository>()
    .AddScoped<ITokenRepository, TokenRepository>()
    .AddScoped<IVerifyAccountRepository, VerifyAccountRepository>()
    .AddScoped<ICustomerRepository, CustomerRepository>();



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
