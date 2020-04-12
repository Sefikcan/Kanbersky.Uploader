using Azure.Storage.Blobs;
using Kanbersky.Uploader.Business.Abstract;
using Kanbersky.Uploader.Business.Concrete.Azure;
using Kanbersky.Uploader.Core.Extensions;
using Kanbersky.Uploader.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Kanbersky.Uploader.Api
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<AzureBlobSettings>(_configuration.GetSection("AzureBlobSettings"));
            services.AddSingleton(new BlobServiceClient(_configuration.GetSection("AzureBlobSettings").GetValue<string>("ConnectionStrings")));
            services.Configure<ElasticSearchSettings>(_configuration.GetSection("ElasticSearchSettings"));

            services.AddScoped<IFileUploaderService, BlobStorageService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Kanbersky.Uploader Microservice",
                    Version = "v1"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseExceptionMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kanbersky Uploader v1");
            });
        }
    }
}
