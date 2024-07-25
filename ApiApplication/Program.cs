
using ApiApplication.Interfaces;
using ApiApplication.Repositories;
using System.Security.AccessControl;

namespace ApiApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IInsuranceProvidersRepository, InsuranceProvidersRepository>();
            builder.Services.AddSingleton<IPatientsRepository,PatientsRepository>();
            
            var app = builder.Build();

            //AddScoped<IInsuranceProvidersRepository, InsuranceProvidersRepository>();
            //AddScoped<IPatientsRepository, PatientsRepository>();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:4200/");
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
            });

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        
    }
}
