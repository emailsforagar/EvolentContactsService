using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Evolent.ContactsService.Models;
using NSwag;


namespace Evolent.ContactsService
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
            var connectionString = Configuration.GetConnectionString("ContactsDatabase");
            services.AddDbContext<dbsegapracdevContext>(options => options.UseSqlServer(connectionString));
            services.AddControllers();
            // Application Insights Telemerty collection
            services.AddApplicationInsightsTelemetry();
            ConfigureSwagger(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3(settings => { settings.TagsSorter = "alpha"; });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        /// <summary>
        /// Configure the swagger document
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        private void ConfigureSwagger(IServiceCollection serviceDescriptors)
        {
            var openApiDocument = Configuration.Get<OpenApiDocument>();

            // Register the Swagger services
            serviceDescriptors.AddSwaggerDocument(config =>
            {
                config.AllowReferencesWithProperties = true;
                config.AlwaysAllowAdditionalObjectProperties = true;
                config.FlattenInheritanceHierarchy = true;
                config.PostProcess = document =>
                {
                    document.Info.Version = openApiDocument.Info.Version;
                    document.Info.Title = openApiDocument.Info.Title;
                    document.Info.Description = openApiDocument.Info.Description;
                    document.Info.TermsOfService = openApiDocument.Info.TermsOfService;
                    document.Info.Contact = new OpenApiContact
                    {
                        Name = openApiDocument.Info.Contact.Name,
                        Email = openApiDocument.Info.Contact.Email,
                        Url = openApiDocument.Info.Contact.Url
                    };

                    document.Info.License = new OpenApiLicense
                    {
                        Name = openApiDocument.Info.License.Name,
                        Url = openApiDocument.Info.License.Url
                    };

                    document.Schemes.Clear();
                    document.Schemes.Add(OpenApiSchema.Https);
                };

                config.SchemaType = NJsonSchema.SchemaType.OpenApi3;
            });
        }
    }
}
