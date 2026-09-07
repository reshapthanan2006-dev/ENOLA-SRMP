using Microsoft.EntityFrameworkCore;
using SRMP.Data;
using SRMP.Interfaces;
using SRMP.Repositories;
using SRMP.Interfaces.Services;
using SRMP.Services;

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

            // Add services to the container.
            builder.Services.AddControllers();

            // Matching Engine
            builder.Services.AddScoped<IMatchingService, MatchingService>();

            // Application services
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();

            // Notification services
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            // Contact request services
            builder.Services.AddScoped<IContactRequestRepository, ContactRequestRepository>();
            builder.Services.AddScoped<IContactRequestService, ContactRequestService>();

            builder.Services.AddScoped<IEmployerCompanyRepository, EmployerCompanyRepository>();
            builder.Services.AddScoped<IEmployerCompanyService, EmployerCompanyService>();

            builder.Services.AddScoped<IJobVacancyRepository, JobVacancyRepository>();
            builder.Services.AddScoped<IJobVacancyService, JobVacancyService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IJobSeekerProfileRepository, JobSeekerProfileRepository>();
            builder.Services.AddScoped<IJobSeekerProfileService, JobSeekerProfileService>();

            builder.Services.AddScoped<IJobSeekerCvRepository, JobSeekerCvRepository>();
            builder.Services.AddScoped<IJobSeekerCvService, JobSeekerCvService>();

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
        }
    }
}