using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VacApp_Bovinova_Platform.RanchManagement.Application.Internal.CommandServices;
using VacApp_Bovinova_Platform.RanchManagement.Application.Internal.QueryServices;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Repositories;
using VacApp_Bovinova_Platform.RanchManagement.Domain.Services;
using VacApp_Bovinova_Platform.RanchManagement.Infrastructure.Persistence.EFC.Repositories;
using VacApp_Bovinova_Platform.Shared.Domain.Repositories;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Interfaces.ASAP.Configuration;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Configuration;
using VacApp_Bovinova_Platform.Shared.Infrastructure.Persistence.EFC.Repositories;
using VacApp_Bovinova_Platform.StaffAdministration.Application.Internal.CommandServices;
using VacApp_Bovinova_Platform.StaffAdministration.Application.Internal.QueryServices;
using VacApp_Bovinova_Platform.StaffAdministration.Domain.Repositories;
using VacApp_Bovinova_Platform.StaffAdministration.Domain.Services;
using VacApp_Bovinova_Platform.StaffAdministration.Infrastructure.Persistence.EFC.Repositories;
using VacApp_Bovinova_Platform.CampaignManagement.Application.Internal.CommandServices;
using VacApp_Bovinova_Platform.CampaignManagement.Application.Internal.QueryServices;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Repositories;
using VacApp_Bovinova_Platform.CampaignManagement.Domain.Services;
using VacApp_Bovinova_Platform.CampaignManagement.Infrastructure.Repositories;
using VacApp_Bovinova_Platform.IAM.Application.OutBoundServices;
using VacApp_Bovinova_Platform.IAM.Domain.Repositories;
using VacApp_Bovinova_Platform.IAM.Domain.Services;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Repositories;
using VacApp_Bovinova_Platform.IAM.Application.CommandServices;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Hashing.BCrypt.Services;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Tokens.JWT.Services;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Tokens.JWT.Configuration;
using Microsoft.OpenApi.Models;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using VacApp_Bovinova_Platform.IAM.Application.QueryServices;
using dotenv.net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using VacApp_Bovinova_Platform.IAM.Infrastructure.OAuth.Google.Services;
using VacApp_Bovinova_Platform.IAM.Infrastructure.OAuth.Google.TokenHandler;
using VacApp_Bovinova_Platform.IAM.Infrastructure.Tokens.Google.Services;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//configure Lower Case URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Configure Kebab Case Route Naming Convention
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options => options.EnableAnnotations());
builder.Services.AddSwaggerGen(
    c =>
    {
        c.EnableAnnotations();

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    });

/////////////////////////Begin Database Configuration/////////////////////////

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Verify Database Connection string
if (connectionString is null)
    throw new Exception("Database connection string is not set");

// Configure Database Context and Logging Levels
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<AppDbContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<AppDbContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableDetailedErrors();
        });

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Bounded Context Injection Configuration for Business

//IAM BC
builder.Services.AddScoped<IUserRepostory, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IGoogleAuthHelper, GoogleAuthHelperService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

// Configuración correcta de autenticación
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSettings:Secret"]!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddScheme<AuthenticationSchemeOptions, GoogleAccessTokenAuthenticationHandler>(
        Constant.Scheme, null)
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;
        options.CallbackPath = $"/{builder.Configuration["Google:RedirectUrl"]}";
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IGoogleAuthHelper, GoogleAuthHelperService>();
builder.Services.AddScoped<IGoogleAuthorization, GoogleAuthorizationService>();

//Ranch Management BC
builder.Services.AddScoped<IBovineRepository, BovineRepository>();
builder.Services.AddScoped<IBovineQueryService, BovineQueryService>();
builder.Services.AddScoped<IBovineCommandService, BovineCommandService>();

builder.Services.AddScoped<IVaccineRepository, VaccineRepository>();
builder.Services.AddScoped<IVaccineQueryService, VaccineQueryService>();
builder.Services.AddScoped<IVaccineCommandService, VaccineCommandService>();

builder.Services.AddScoped<IStableRepository, StableRepository>();
builder.Services.AddScoped<IStableQueryService, StableQueryService>();
builder.Services.AddScoped<IStableCommandService, StableCommandService>();

//Staff Administration BC
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffQueryService, StaffQueryService>();
builder.Services.AddScoped<IStaffCommandService, StaffCommandService>();

//Campaign Management BC
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignCommandService, CampaignCommandService>();
builder.Services.AddScoped<ICampaignQueryService, CampaignQueryService>();


/////////////////////////End Database Configuration/////////////////////////
var app = builder.Build();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllPolicy");

//app.UseRequestAuthorization();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
