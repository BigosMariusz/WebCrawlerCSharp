using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using WebCrawler.Data;
using Nito.AsyncEx;
using WebCrawler.Services;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
                AsyncContext.Run(() => MainAsync());
        }

        static async void MainAsync()
        {
            while (true)
            {
                Console.WriteLine("1. Zrób nowe przeszukiwanie sieci. (Usuwa dane z poprzedniego przeszukiwania)");
                Console.WriteLine("2. Skonfiguruj połączenie z bazą danych.");

                int mainMenuOption;
                var tryMainMenuOption = Int32.TryParse(Console.ReadLine(), out mainMenuOption);
                if (tryMainMenuOption)
                {
                    var menu = new Menu();
                    switch (mainMenuOption)
                    {
                        case 1:
                            await menu.startCrawlerConfigAsync();
                            break;
                        case 2:
                            menu.takeDataBaseSettigs();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Taka opcja nie istnieje.");
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Podana wartość nie jest liczbą \n");
                }
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = DbService.GetConnectionString();
            var mysqlVersion = DbService.GetMysqlVersion();

            services.AddDbContextPool<CrawlerContext>( 
                options => options.UseMySql(connectionString, 
                    mysqlOptions =>
                    {
                        mysqlOptions.ServerVersion(new Version(mysqlVersion), ServerType.MySql); 
                    }
            ));
        }
    }
}
