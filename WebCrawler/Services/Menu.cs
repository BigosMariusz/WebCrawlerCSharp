using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.LocalDataFormats;
using WebCrawler.Services;

namespace WebCrawler
{
    class Menu
    {
        public async Task startCrawlerConfigAsync()
        {
            Console.Clear();

            var isConnectDb = await DbService.CheckDbConnectionAsync();
            if (!isConnectDb)
            {
                Console.WriteLine("Nie działa połączenie z bazą danych bądź serwer MySql nie jest włączony.\n");
                return;
            }

            string url;
            int hopSize = 0;
            while (true)
            {
                Console.WriteLine("Podaj adres url w formacie http(s)://example.com:");
                url = Console.ReadLine();
                if (LinkService.IsValidLinkFromUser(url))
                {
                    break;
                }
                Console.Clear();
                Console.WriteLine("Podano zły link. Prawidłowy format to http(s)://example.com. \n");
            }
            while (true)
            {
                Console.WriteLine("Podaj ilość skoków:");
                var tryHopSize = Int32.TryParse(Console.ReadLine(), out hopSize);
                if (tryHopSize)
                {
                    break;
                }
                Console.Clear();
                Console.WriteLine("Podana wartość skoków nie jest liczbą całkowitą. \n");
            }
            while (true)
            {
                Console.WriteLine("Podaj długość przerwy jaka będzie pomiędzy 100 wejściami na strony (w sekundach):");
                int pauseLength;
                var tryPauseLength = Int32.TryParse(Console.ReadLine(), out pauseLength);
                if (tryPauseLength)
                {
                    var crawler = new Crawler();

                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    int numberOfLinks = await crawler.CrawlAsync(url, hopSize, pauseLength);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;

                    Console.Clear();
                    Console.WriteLine($"Przeszukiwanie zakończone. \nCzas przeszukiwania (w sekundach) to: {elapsedMs / 1000}. \nIlość znalezionych linków to: {numberOfLinks}. \n");
                    Console.WriteLine("Wciśnij enter aby przejść do menu.");
                    Console.ReadKey();
                    break;
                }
                Console.Clear();
                Console.WriteLine("Podana wartość nie jest liczbą");
            }
            Console.Clear();
        }

        public void takeDataBaseSettigs()
        {
            Console.Clear();

            Console.WriteLine("Podaj adres serwera bazy danych (np. localhost lub 127.0.0.1):");
            var serverAddress = Console.ReadLine();

            Console.WriteLine("Podaj nazwę bazy danych (jeśli baza nie istnieje zostanie utworzona):");
            var dbName = Console.ReadLine();

            Console.WriteLine("Podaj nazwę użytkownika serwera bazodanowego:");
            var dbUser = Console.ReadLine();

            Console.WriteLine("Podaj hasło użytkownika serwera bazodanowego:");
            var dbPassword = Console.ReadLine();

            Console.WriteLine("Podaj wersję serwera bazy danych MySql (format: 8.0.13):");
            var mysqlVersion = Console.ReadLine();

            var dbSettings = new DbSettings
            {
                ServerAddress = serverAddress,
                DbName = dbName,
                DbUser = dbUser,
                DbPassword = dbPassword,
                MysqlVersion = mysqlVersion
            };

            DbService.ChangeDbSettings(dbSettings);

            Console.Clear();
        }
    }
}
