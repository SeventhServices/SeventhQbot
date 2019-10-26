using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeventhServices.QQRobot.Client.Formats;
using SeventhServices.QQRobot.Client.Interface;
using SeventhServices.QQRobot.Services;
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
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });
            services.AddHttpApi<IQqLightClient>().ConfigureHttpApiConfig(config =>
            {
                config.JsonFormatter = new JsonTextFormatter();
            });
            services.AddSingleton<SendMessageService>();
            services.AddSingleton<RandomService>();
            services.AddSingleton<RandomRepeat>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
