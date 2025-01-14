using Amazon.SimpleEmail;
using book_a_reading_room_visit.api.Service;
using book_a_reading_room_visit.data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net.Http.Headers;

namespace book_a_reading_room_visit.api
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
            services.AddControllers();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddDataProtection().PersistKeysToAWSSystemsManager("/KBS-API/DataProtection");
            services.AddAWSService<IAmazonSimpleEmailService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddLogging(config =>
            {
                config.AddAWSProvider(Configuration.GetAWSLoggingConfigSection());
                config.SetMinimumLevel(LogLevel.Debug);
            });

            services.AddDbContext<BookingContext>(opt =>
              opt.UseSqlServer(Environment.GetEnvironmentVariable("KewBookingConnection"))
                 .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddMemoryCache();
            services.AddHttpClient<IBankHolidayAPI, BankHolidayAPI>(c =>
            {
                c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("RecordCopying_WebApi_URL"));
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                c.DefaultRequestHeaders.Host = Environment.GetEnvironmentVariable("RecordCopying_Header");
            });
            services.AddScoped<IWorkingDayService, WorkingDayService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IBookingService, BookingService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book a reading room visit-api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book a reading room visit-api v1"));
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
