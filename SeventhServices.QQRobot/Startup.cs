using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using SeventhServices.Asset.LocalDB;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Client.Abstractions;
using SeventhServices.QQRobot.Client.Formats;
using SeventhServices.QQRobot.Services;
using SqlParse.Classes;
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
            services.AddSingleton<MessageParser>();
            services.AddSingleton<LocalDbLoader>();

            services.AddSingleton<IRepository<Card>,CardRepository>();


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
                foreach (var xml in Configuration.GetSection("XmlDocuments").GetChildren())
                {
                    options.IncludeXmlComments(
                        Path.Combine(
                            Path.GetDirectoryName(
                                Assembly.GetEntryAssembly()?.Location),
                            xml.Value));
                }

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
            }) ;

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
