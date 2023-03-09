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
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(options => options.AddProfile(typeof(MapperProfile)));
var localConnectionString = string.Empty;
localConnectionString = builder.Configuration.GetConnectionString("LocalConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(localConnectionString, b =>
    {
        b.MigrationsAssembly("BookEcommerce");
    });
    //options.UseLazyLoadingProxies();
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
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
//DB
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>()
//.AddScoped<DbFactory>();

services.AddScoped((Func<IServiceProvider, Func<ApplicationDbContext>>)((provider) => () => provider.GetService<ApplicationDbContext>()));
services.AddScoped<DbFactory>();
services.AddScoped(typeof(BookEcommerce.Models.DAL.Interfaces.IRepository<>), typeof(Repository<>));
services.AddScoped<IUnitOfWork, UnitOfWork>();

//service
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<IProductService, ProductService>();
services.AddScoped<ICartService, CartService>();
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<ICartDetailService, CartDetailService>();
services.AddScoped<ISearchService, SearchService>();
//repo
builder.Services.AddScoped<IRoleRepository, RoleRepository>()
.AddScoped<AutoMapper.Profile, MapperProfile>()
//.AddScoped<IMapper, Mapper>()
.AddScoped<IAuthenticationRepository, AuthenticationRepository>()
.AddScoped<ITokenRepository, TokenRepository>();
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<ICartRepository, CartRepository>();
services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
services.AddScoped<IImageRepository, ImageRepository>();
services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
services.AddScoped<ICartDetailRepository, CartDetailRepository>();
services.AddScoped<IOrderRepository, OrderRepository>();
services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
services.AddScoped<ICategoryRepository, CategoryRepository>();


var app = builder.Build();

app.UseHttpLogging();

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
