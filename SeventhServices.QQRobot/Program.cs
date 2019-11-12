using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace SeventhServices.QQRobot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }


        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:65321");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
