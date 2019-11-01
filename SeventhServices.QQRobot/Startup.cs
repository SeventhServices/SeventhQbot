using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Client.Abstractions;
using SeventhServices.QQRobot.Client.Formats;
using SeventhServices.QQRobot.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiClient.Defaults;
using WebApiClient.Extensions.DependencyInjection;


namespace SeventhServices.QQRobot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });
            services.AddHttpApi<IQqLightClient>().ConfigureHttpApiConfig(config =>
            {
                config.JsonFormatter = new JsonTextFormatter();
            });
            services.AddSingleton<SendMessageService>();
            services.AddSingleton<RandomService>();
            services.AddSingleton<RandomRepeat>();
            services.AddSingleton<IMessagePipeline,SimpleReturnPipeline>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Seventh QQRobot",
                        Contact = new OpenApiContact(),
                        Description = " "
                    });
            });


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
            });

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
