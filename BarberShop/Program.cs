using BarberShop.Data;
using BarberShop.Entity;
using BarberShop.Repository;
using BarberShop.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Vui lòng nhập token vào ô Value (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


builder.Services.AddCors(p => p.AddPolicy("BarberShop", build =>
{
    //build.WithOrigins("https://khoahoc.info", "https://localhost:7224", "http://localhost:3003/");
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddDbContext<BarberShopContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("BarberShop"));
});

#region Mapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Đăng k? d?ch v? IRepository và GenericRepository cho t?ng lo?i
builder.Services.AddScoped<IRepository<Address>, GenericRepository<Address>>();
builder.Services.AddScoped<IRepository<Booking>, GenericRepository<Booking>>();
builder.Services.AddScoped<IRepository<BookingStateDescription>, GenericRepository<BookingStateDescription>>();
//builder.Services.AddScoped<IRepository<BookingService>, GenericRepository<BookingService>>();
builder.Services.AddScoped<IRepository<Category>, GenericRepository<Category>>();
builder.Services.AddScoped<IRepository<City>, GenericRepository<City>>();
builder.Services.AddScoped<IRepository<Country>, GenericRepository<Country>>();
builder.Services.AddScoped<IRepository<Customer>, GenericRepository<Customer>>();
builder.Services.AddScoped<IRepository<CustomerAddress>, GenericRepository<CustomerAddress>>();
builder.Services.AddScoped<IRepository<CustomerNotification>, GenericRepository<CustomerNotification>>();
builder.Services.AddScoped<IRepository<Employee>, GenericRepository<Employee>>();
builder.Services.AddScoped<IRepository<Evaluate>, GenericRepository<Evaluate>>();
builder.Services.AddScoped<IRepository<LocationStore>, GenericRepository<LocationStore>>();
builder.Services.AddScoped<IRepository<Notification>, GenericRepository<Notification>>();
builder.Services.AddScoped<IRepository<Order>, GenericRepository<Order>>();
builder.Services.AddScoped<IRepository<Payment>, GenericRepository<Payment>>();
builder.Services.AddScoped<IRepository<Producer>, GenericRepository<Producer>>();
builder.Services.AddScoped<IRepository<Product>, GenericRepository<Product>>();
builder.Services.AddScoped<IRepository<ProductOrder>, GenericRepository<ProductOrder>>();
builder.Services.AddScoped<IRepository<Role>, GenericRepository<Role>>();
builder.Services.AddScoped<IRepository<Services>, GenericRepository<Services>>();
builder.Services.AddScoped<IRepository<ServiceCategory>, GenericRepository<ServiceCategory>>();
builder.Services.AddScoped<IRepository<ServiceManagement>, GenericRepository<ServiceManagement>>();
builder.Services.AddScoped<IRepository<Store>, GenericRepository<Store>>();
builder.Services.AddScoped<IRepository<User>, GenericRepository<User>>();
builder.Services.AddScoped<IRepository<Warehouse>, GenericRepository<Warehouse>>();
builder.Services.AddScoped<IRepository<WorkingHour>, GenericRepository<WorkingHour>>();
#endregion

#region JWT

#endregion

builder.Services.AddScoped<BarberShop.Unit.UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("BarberShop");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
