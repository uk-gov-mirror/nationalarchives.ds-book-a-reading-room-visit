using System;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Xml;
using book_a_reading_room_visit.web.Helper;
using book_a_reading_room_visit.web.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace book_a_reading_room_visit.web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _currentEnvironment;
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            _currentEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.ModelBinderProviders.Insert(0, new EnumModelBinderProvider());
            });
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddDataProtection().PersistKeysToAWSSystemsManager("/KBS-Web/DataProtection");
            services.AddLogging(config =>
            {
                config.AddAWSProvider(Configuration.GetAWSLoggingConfigSection());
                config.SetMinimumLevel(LogLevel.Debug);
            });

            services.AddHttpClient<IAvailabilityService, AvailabilityService>(c =>
            {
                c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("KBS_WebApi_URL"));
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddHttpClient<IBookingService, BookingService>(c =>
            {
                c.BaseAddress = new Uri(Environment.GetEnvironmentVariable("KBS_WebApi_URL"));
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            var wcfAdvanceOrderServiceEndPoint = Environment.GetEnvironmentVariable("AdvanceOrderServiceEndPoint");
            services.AddSingleton(s => new ChannelFactory<IAdvancedOrderService>(new BasicHttpBinding(), new EndpointAddress(wcfAdvanceOrderServiceEndPoint)));

            var wcfBulkOrderServiceEndPoint = Environment.GetEnvironmentVariable("BulkOrderServiceEndPoint");
            services.AddSingleton(s => new ChannelFactory<IBulkOrdersService>(new BasicHttpBinding(), new EndpointAddress(wcfBulkOrderServiceEndPoint)));

            services.AddScoped<ValidateDocumentOrder>();
            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseSecurityHeaderMiddleware();
            app.UseRouting();
            var rootPath = Environment.GetEnvironmentVariable("KBS_Root_Path");
            if (!string.IsNullOrWhiteSpace(rootPath))
            {
                app.Use((context, next) =>
                {
                    context.Request.PathBase = new PathString(rootPath);
                    return next();
                });
            }
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "",
                    new { controller = "Home", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "availability",
                    pattern: "{bookingtype}/availability",
                    new { controller = "Home", action = "Availability" });

                endpoints.MapControllerRoute(
                    name: "secure-booking",
                    pattern: "{bookingtype}/secure-booking",
                    new { controller = "Booking", action = "SecureBooking" });

                endpoints.MapControllerRoute(
                    name: "booking-confirmation",
                    pattern: "{bookingtype}/booking-confirmation",
                    new { controller = "Booking", action = "BookingConfirmation" });

                endpoints.MapControllerRoute(
                    name: "cancel-provision",
                    pattern: "{bookingtype}/cancel-provision",
                    new { controller = "Booking", action = "CancelProvision" });

                endpoints.MapControllerRoute(
                    name: "cancel-booking",
                    pattern: "{bookingtype}/cancel-booking",
                    new { controller = "Booking", action = "CancelBooking" });

                endpoints.MapControllerRoute(
                    name: "cancellation-confirmation",
                    pattern: "{bookingtype}/cancellation-confirmation",
                    new { controller = "Booking", action = "CancellationConfirmation" });

                endpoints.MapControllerRoute(
                    name: "order-documents",
                    pattern: "{bookingtype}/order-documents/{readerticket}/{bookingreference}",
                    new { controller = "DocumentOrder", action = "OrderDocuments" });

                endpoints.MapControllerRoute(
                    name: "document-order",
                    pattern: "{bookingtype}/document-order/{readerticket}/{bookingreference}",
                    new { controller = "DocumentOrder", action = "OrderComplete" });

                endpoints.MapControllerRoute(
                    name: "return-to-booking",
                    pattern: "return-to-booking",
                    new { controller = "Booking", action = "ReturnToBooking" });

                endpoints.MapControllerRoute(
                    name: "continue-later",
                    pattern: "{bookingtype}/continue-later/{bookingreference}",
                    new { controller = "DocumentOrder", action = "ContinueLater" });

                endpoints.MapControllerRoute(
                   name: "thank-you",
                   pattern: "{bookingtype}/thank-you",
                   new { controller = "Booking", action = "ThankYou" });

                endpoints.MapControllerRoute(
                   name: "about-the-book-a-reading-room-visit-service",
                   pattern: "about-the-book-a-reading-room-visit-service",
                   new { controller = "Home", action = "AboutTheBookAReadingRoomVisitService" });

                endpoints.MapControllerRoute(
                   name: "privacy-policy",
                   pattern: "privacy-policy",
                   new { controller = "Home", action = "PrivacyPolicy" });

                endpoints.MapControllerRoute(
                   name: "terms-of-use",
                   pattern: "terms-of-use",
                   new { controller = "Home", action = "TermsOfUse" });

                endpoints.MapControllerRoute(
                   name: "what-can-I-expect-when-I-visit",
                   pattern: "what-can-I-expect-when-I-visit",
                   new { controller = "Home", action = "WhatCanIExpectWhenIVisit" });

                endpoints.MapControllerRoute(
                   name: "error",
                   pattern: "error",
                   new { controller = "Home", action = "Error" });
            });
        }
    }
}
