using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SWBackend.DataBase;
using SWBackend.Models.SignUp.Identity;
using SWBackend.RepositoryLayer;
using SWBackend.RepositoryLayer.IRepository.User;
using SWBackend.ServiceLayer;
using SWBackend.ServiceLayer.Auth;
using SWBackend.ServiceLayer.IService.IUserService;
using SWBackend.ServiceLayer.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<SWDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IJwtService, JwtService>();

//TODO: In fasi finali cambiare il numero minimo richiesto da 6 ad 8/12
builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
{
    //IMPOSTAZIONI PER EMAIL
    options.SignIn.RequireConfirmedEmail = true; 
    options.User.RequireUniqueEmail = true;

    //IMPOSTAZIONI PER PASSWORD
    options.Password.RequireDigit = true;               // Deve contenere almeno un numero (0-9)
    options.Password.RequiredLength = 6;                // Lunghezza minima (default = 6)
    options.Password.RequireNonAlphanumeric = true;     // Deve contenere almeno un simbolo (!, @, #, etc.)
    options.Password.RequireUppercase = true;           // Deve contenere almeno una maiuscola
    options.Password.RequireLowercase = true;           // Deve contenere almeno una minuscola
    options.Password.RequiredUniqueChars = 1;           // Minimo numero di caratteri unici
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

})
.AddEntityFrameworkStores<SWDbContext>()  // usa il tuo DbContext con Identity
.AddDefaultTokenProviders();

// builder.Services
//     .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
//                 builder.Configuration["Jwt:Key"]!
//             )),
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             RoleClaimType = ClaimTypes.Role 
//         };
//     });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

        RoleClaimType = ClaimTypes.Role, // <- importantissimo!
        NameClaimType = ClaimTypes.Name
    };
});

builder.Services.AddTransient<IEmailerSender, SmtpEmailSender>();


builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "SW_Jwt",
            Version = "v1"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Here insert JWT with bearer format: bearer[sapce] token"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme{
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string [] { }
            }
        });
});

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

app.MapControllers();


app.Run();

