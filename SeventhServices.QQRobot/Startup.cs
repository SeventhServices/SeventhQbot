using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SeventhServices.Asset;
using SeventhServices.Asset.LocalDB;
using SeventhServices.Asset.LocalDB.Classes;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Abstractions.Services;
using SeventhServices.QQRobot.Client.Abstractions;
using SeventhServices.QQRobot.Client.Formats;
using SeventhServices.QQRobot.Parser;
using SeventhServices.QQRobot.Repositories;
using SeventhServices.QQRobot.Resource;
using SeventhServices.QQRobot.Resource.Repositories;
using SeventhServices.QQRobot.Services;
using SeventhServices.QQRobot.Workers;
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
            var localDirectoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });

            services.AddHttpApi<IQqLightClient>().ConfigureHttpApiConfig(config =>
            {
                config.JsonFormatter = new JsonTextFormatter();
            });

            services.AddDbContext<QBotDbContext>(option =>
                option.UseSqlite("Data Source=bot.db"),
                ServiceLifetime.Singleton);
            services.AddSingleton<BindRepository>();
            services.AddSingleton<MessageParser>();
            services.AddSingleton<IMessagePipeline,SimpleReturnPipeline>();

            services.AddSingleton<ISendMessageService,QqLightSendMessageService>();
            services.AddSingleton<RandomService>();
            services.AddSingleton<RandomRepeat>();
            services.AddSingleton<LocalDbLoader>();
            services.AddSingleton<IRepository<Card>, CardRepository>();
            services.AddSingleton<IRepository<Character>, CharacterRepository>();
            services.AddRobotCommandParser();
            services.AddSeventhAssetServices();

            services.AddHostedService<AutoSendWorker>();

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
                        Path.Combine(localDirectoryName, xml.Value));
                }
            });


        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseStaticFiles();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
