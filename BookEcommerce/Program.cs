using BookEcommerce.Models.DAL;
using BookEcommerce.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BookEcommerce.Services.Interfaces;
using BookEcommerce.Services;
using System.Text;
using BookEcommerce.Services.Map;
using BookEcommerce.Models.DAL.Interfaces;
using BookEcommerce.Models.DAL.Repositories;
using AutoMapper;
using MimeKit;
using MailKit.Net.Smtp;

using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions();
var services = builder.Services;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "paypal-session";
    options.Cookie.IsEssential = true;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAutoMapper(options => options.AddProfile(typeof(MapperProfile)));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"), b => b.MigrationsAssembly("BookEcommerce"));
    options.UseLazyLoadingProxies();
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

//builder.Services.AddHttpLogging(logging =>
//{
//    logging.LoggingFields = HttpLoggingFields.All;
//    logging.RequestHeaders.Add("sec-ch-ua");
//    logging.ResponseHeaders.Add("MyResponseHeader");
//    logging.MediaTypeOptions.AddText("application/javascript");
//    logging.RequestBodyLogLimit = 4096;
//    logging.ResponseBodyLogLimit = 4096;
//});

//builder.Host.ConfigureLogging(logging =>
//{
//    logging.AddConsole();
//    logging.ClearProviders();
//});


//DB
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>()
//.AddScoped<DbFactory>();

services.AddScoped((Func<IServiceProvider, Func<ApplicationDbContext>>)((provider) => () => provider.GetService<ApplicationDbContext>()));
services.AddScoped<DbFactory>();
services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<ILogger, Logger<PaymentService>>();

builder.Services
    .AddScoped<MailSettings>()
    .AddScoped<MimeMessage>()
    .AddScoped<SmtpClient>();

//service
builder.Services
    .AddScoped<IAuthenticationService, AuthenticationService>()
    .AddScoped<IVerifyAccountService, VerifyAccountService>()
    .AddScoped<ITokenService, TokenService>()
    .AddScoped<ICustomerService, CustomerService>()
    .AddScoped<IVendorService, VendorService>()
    .AddScoped<IProductService, ProductService>()
    .AddScoped<ICartService, CartService>()
    .AddScoped<IAddressService, AddressService>()
    .AddScoped<IOrderService, OrderService>()
    .AddScoped<IPaymentService, PaymentService>();
//repo
builder.Services
    .AddScoped<IRoleRepository, RoleRepository>()
    //.AddScoped<Profile, MapperProfile>()
    .AddScoped<IMapper, Mapper>()
    .AddScoped<ISendMailRepository, SendMailRepository>()
    .AddScoped<IAuthenticationRepository, AuthenticationRepository>()
    .AddScoped<IRoleRepository, RoleRepository>()
    .AddScoped<ITokenRepository, TokenRepository>()
    .AddScoped<IVerifyAccountRepository, VerifyAccountRepository>()
    .AddScoped<IVendorRepository, VendorRepository>()
    .AddScoped<ICustomerRepository, CustomerRepository>()
    .AddScoped<IAddressRepository, AddressRepository>();
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
services.AddScoped<ICartRepository, CartRepository>();
services.AddScoped<ICartDetailRepository, CartDetailRepository>();
services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
services.AddScoped<IImageRepository, ImageRepository>();
services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
services.AddScoped<IPaymentRepository, PaymentRepository>();
var app = builder.Build();

app.UseHttpLogging();

//app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
