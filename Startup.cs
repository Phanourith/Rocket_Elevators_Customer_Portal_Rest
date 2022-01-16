using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Rocket_Elevators_REST_API.Models;
using Npgsql;

namespace Rocket_Elevators_REST_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            // Postgres connection string
            
            string connString =
                String.Format(
                    "Server={0};Username={1};Database={2};Port={3};Password={4};",
                    "codeboxx-postgresql.cq6zrczewpu2.us-east-1.rds.amazonaws.com",
                    "codeboxx",
                    "jkg",
                    "5432",
                    "Codeboxx1!");

            using (var conn = new NpgsqlConnection(connString)) 

            services.AddDbContext<PostgreApplicationContext>(options => options.UseNpgsql(connString));

            // MySQL Connection String

            // NOTE: USE ACTUAL CONNECTION STRING IN CONNECTION FOR PRODUCTION WHEN DEPLOYING!!!
            var connectionString =  @"server=db-mysql-nyc3-70101-do-user-10631197-0.b.db.ondigitalocean.com;userid=doadmin;password=sjNxhA9AVidUmB78;database=LongNguyen;port=25060";

            // Replace with your server version and type.
            // Use 'MariaDbServerVersion' for MariaDB.
            // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
            // For common usages, see pull request #1233.
            var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));


            // Replace 'YourDbContext' with the name of your own DbContext derived class.
            services.AddDbContext<ApplicationContext>(options =>
                options.UseMySql(connectionString, serverVersion)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //     app.UseSwagger();
                //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rocket_Elevators_REST_API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}