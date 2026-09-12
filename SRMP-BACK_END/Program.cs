using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SRMP.Data;
using SRMP.Helpers;
using SRMP.Interfaces;
using SRMP.Interfaces.Services;
using SRMP.Middelware;
using SRMP.Repositories;
using SRMP.Services;
using System.Text;

namespace SRMP
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database
            builder.Services.AddDbContext<AppDbContext>(
                options =>
                    options.UseSqlServer(
                        builder.Configuration
                            .GetConnectionString("DefaultConnection")));

            // Controllers
            builder.Services.AddControllers();

            // CORS for Angular frontend
            const string FrontendCorsPolicy =
                "FrontendCorsPolicy";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    FrontendCorsPolicy,
                    policy =>
                    {
                        policy
                            .WithOrigins(
                                "http://localhost:4200",
                                "https://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // Authentication services
            builder.Services.AddScoped<
                IAuthRepository,
                AuthRepository>();

            builder.Services.AddScoped<
                IAuthService,
                AuthService>();

            // Admin services
            builder.Services.AddScoped<
                IAdminRepository,
                AdminRepository>();

            builder.Services.AddScoped<
                IAdminService,
                AdminService>();

            // JWT Helper
            builder.Services.AddScoped<JwtHelper>();

            // JWT Authentication
            builder.Services
                .AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtKey =
                        builder.Configuration["Jwt:Key"];

                    if (string.IsNullOrWhiteSpace(jwtKey))
                    {
                        throw new InvalidOperationException(
                            "JWT key is not configured.");
                    }

                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(jwtKey)),

                            ValidateIssuer = true,

                            ValidIssuer =
                                builder.Configuration["Jwt:Issuer"],

                            ValidateAudience = true,

                            ValidAudience =
                                builder.Configuration["Jwt:Audience"],

                            ValidateLifetime = true,

                            ClockSkew = TimeSpan.Zero
                        };
                });

            builder.Services.AddAuthorization();

            // Matching Engine
            builder.Services.AddScoped<
                IMatchingService,
                MatchingService>();

            // Application
            builder.Services.AddScoped<
                IApplicationRepository,
                ApplicationRepository>();

            builder.Services.AddScoped<
                IApplicationService,
                ApplicationService>();

            // Notification
            builder.Services.AddScoped<
                INotificationRepository,
                NotificationRepository>();

            builder.Services.AddScoped<
                INotificationService,
                NotificationService>();

            // Contact Request
            builder.Services.AddScoped<
                IContactRequestRepository,
                ContactRequestRepository>();

            builder.Services.AddScoped<
                IContactRequestService,
                ContactRequestService>();

            // Employer Company
            builder.Services.AddScoped<
                IEmployerCompanyRepository,
                EmployerCompanyRepository>();

            builder.Services.AddScoped<
                IEmployerCompanyService,
                EmployerCompanyService>();

            // Job Vacancy
            builder.Services.AddScoped<
                IJobVacancyRepository,
                JobVacancyRepository>();

            builder.Services.AddScoped<
                IJobVacancyService,
                JobVacancyService>();

            // Job Seeker Profile
            builder.Services.AddScoped<
                IJobSeekerProfileRepository,
                JobSeekerProfileRepository>();

            builder.Services.AddScoped<
                IJobSeekerProfileService,
                JobSeekerProfileService>();

            // Job Seeker CV
            builder.Services.AddScoped<
                IJobSeekerCvRepository,
                JobSeekerCvRepository>();

            builder.Services.AddScoped<
                IJobSeekerCvService,
                JobSeekerCvService>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Name = "Authorization",

                        Type =
                            Microsoft.OpenApi.Models
                                .SecuritySchemeType.Http,

                        Scheme = "bearer",

                        BearerFormat = "JWT",

                        In =
                            Microsoft.OpenApi.Models
                                .ParameterLocation.Header,

                        Description = "Enter JWT token"
                    });

                options.AddSecurityRequirement(
                    new Microsoft.OpenApi.Models
                        .OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models
                                .OpenApiSecurityScheme
                            {
                                Reference =
                                    new Microsoft.OpenApi.Models
                                        .OpenApiReference
                                    {
                                        Type =
                                            Microsoft.OpenApi.Models
                                                .ReferenceType
                                                .SecurityScheme,

                                        Id = "Bearer"
                                    }
                            },

                            Array.Empty<string>()
                        }
                    });
            });

            var app = builder.Build();

            // Seed administrator account if configured
            using (var scope = app.Services.CreateScope())
            {
                var context =
                    scope.ServiceProvider
                        .GetRequiredService<AppDbContext>();

                await DbSeeder.SeedAdminAsync(
                    context,
                    app.Configuration);
            }

            // Global Exception Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Angular CORS
            app.UseCors(FrontendCorsPolicy);

            // Authentication
            app.UseAuthentication();

            // Authorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}