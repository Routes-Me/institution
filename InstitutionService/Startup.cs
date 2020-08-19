using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionService.Abstraction;
using InstitutionService.Helper.Abstraction;
using InstitutionService.Helper.Models;
using InstitutionService.Helper.Repository;
using InstitutionService.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InstitutionService
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

            services.AddDbContext<InstitutionService.Models.DBModels.institutionserviceContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<SendGridSettings>(Configuration.GetSection("SendGridSettings"));
            services.AddScoped<IHelperRepository, HelperRepository>();
            services.AddScoped<IInstitutionRepository, InstitutionRepository>();
            services.AddScoped<IInvitationsRepository, InvitationsRepository>();
            services.AddScoped<IOfficersRepository, OfficersRepository>();
            services.AddScoped<IServicesInstitutionsRepository, ServicesInstitutionsRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IMessageSender, MessageSender>();

            services.AddSingleton<IMessageSender>(new MessageSender(
                Configuration.GetSection("TwilioSMS").Get<Configuration.TwilioSMS>()));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
