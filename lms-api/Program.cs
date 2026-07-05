using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using lms_data;
using lms_data.Repositories;
using lms_service.Implementations;
using lms_service.Interfaces;
using lms_service.Mappings;
using lms_service.Middleware;
using lms_service.Profiles;
using lms_service.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using lms_service.Helpers;
using Microsoft.OpenApi.Models;
using lms_data.Entities;
using Microsoft.AspNetCore.Identity;
using lms_api.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddAutoMapper(typeof(LibraryProfile).Assembly);

// Configure DbContext - SQLite
builder.Services.AddDbContext<LibraryContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=library.db"));


// Register generic repository for entities used by services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IBorrowRecordRepository, BorrowRecordRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
// Services
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();

//auth
builder.Services.AddScoped<IAuthService, AuthService>();

//Profiles
builder.Services.AddAutoMapper(typeof(BookProfile));
builder.Services.AddAutoMapper(typeof(MemberProfile));
builder.Services.AddAutoMapper(typeof(BorrowRecordProfile));
builder.Services.AddAutoMapper(typeof(MappingProfile));

//validators
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<MemberValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BookValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BorrowRequestValidator>();

builder.Services.AddScoped<JwtTokenGenerator>();

var jwt = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwt["Issuer"],
                ValidAudience = jwt["Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt["Key"]!))
    };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.NoResult();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        message = "Invalid or expired token"
                    })
                );
            },

            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        message = "You are not authenticated. Please login first."
                    })
                );
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageBooks", policy =>
        policy.RequireRole("Admin", "Librarian"));

    options.AddPolicy("CanBorrowBooks", policy =>
        policy.RequireRole("Member", "Librarian"));

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();

public partial class Program
{
}