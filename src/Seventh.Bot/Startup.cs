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
using Sagilio.Bot.Parser;
using Seventh.Bot.Abstractions;
using Seventh.Bot.Abstractions.Services;
using Seventh.Bot.Client.Abstractions;
using Seventh.Bot.Client.Formats;
using Seventh.Bot.Repositories;
using Seventh.Bot.Resource;
using Seventh.Bot.Resource.Repositories;
using Seventh.Bot.Services;
using Seventh.Bot.Workers;
using Seventh.Resource.Asset.SqlLoader;
using Seventh.Resource.Asset.SqlLoader.Classes;
using SeventhServices.Resource;
using WebApiClient.Extensions.DependencyInjection;

namespace Seventh.Bot
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
                    Description = ""
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

            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
