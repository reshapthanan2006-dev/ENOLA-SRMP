using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SRMP.Data;
using SRMP.Helpers;
using SRMP.Interfaces;
using SRMP.Interfaces.Services;
using SRMP.Repositories;
using SRMP.Services;
using System.Text;

namespace SRMP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            // Add services to the container
            builder.Services.AddControllers();

            // Authentication services - mem 1
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Admin services - mem 1
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();

            // JWT Helper - mem 1
            builder.Services.AddScoped<JwtHelper>();

            // JWT Authentication - mem 1
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtKey = builder.Configuration["Jwt:Key"];

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

            // CORS for Angular frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularFrontend", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Matching Engine
            builder.Services.AddScoped<IMatchingService, MatchingService>();

            // Application services
            builder.Services.AddScoped<
                IApplicationRepository,
                ApplicationRepository>();

            builder.Services.AddScoped<
                IApplicationService,
                ApplicationService>();

            // Notification services
            builder.Services.AddScoped<
                INotificationRepository,
                NotificationRepository>();

            builder.Services.AddScoped<
                INotificationService,
                NotificationService>();

            // Contact request services
            builder.Services.AddScoped<
                IContactRequestRepository,
                ContactRequestRepository>();

            builder.Services.AddScoped<
                IContactRequestService,
                ContactRequestService>();

            // Employer company services
            builder.Services.AddScoped<
                IEmployerCompanyRepository,
                EmployerCompanyRepository>();

            builder.Services.AddScoped<
                IEmployerCompanyService,
                EmployerCompanyService>();

            // Job vacancy services
            builder.Services.AddScoped<
                IJobVacancyRepository,
                JobVacancyRepository>();

            builder.Services.AddScoped<
                IJobVacancyService,
                JobVacancyService>();

            // Job Seeker Profile services
            builder.Services.AddScoped<
                IJobSeekerProfileRepository,
                JobSeekerProfileRepository>();

            builder.Services.AddScoped<
                IJobSeekerProfileService,
                JobSeekerProfileService>();

            // Job Seeker CV services
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
                            Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In =
                            Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Enter JWT token"
                    });

                options.AddSecurityRequirement(
                    new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                    {
                        {
                            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                            {
                                Reference =
                                    new Microsoft.OpenApi.Models.OpenApiReference
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

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Allow Angular frontend
            app.UseCors("AngularFrontend");

            // JWT Authentication must come before Authorization
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}