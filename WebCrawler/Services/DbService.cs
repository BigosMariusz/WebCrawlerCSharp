using System;
using System.Collections.Generic;
using System.Text;
using WebCrawler.LocalDataFormats;
using Newtonsoft.Json;
using System.IO;
using WebCrawler.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace WebCrawler.Services
{
    public static class DbService
    {
        public static async Task<bool> CheckDbConnectionAsync()
        {
            using (var context = new CrawlerContext())
            {
                try
                {
                    context.Database.EnsureCreated();
                    await context.Links.FirstOrDefaultAsync();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        public static List<DbLink> Get100Links(int tier, int position, int tierLength)
        {
            int numberOfLinks;
            if (tierLength - position >= 100)
            {
                numberOfLinks = 100;
            }
            else
            {
                numberOfLinks = tierLength - position;
            }

            List<DbLink> links;

            using (var context = new CrawlerContext())
            {
                links = context.Links
                        .Where(link => link.Tier == tier)
                        .Skip(position)
                        .Take(numberOfLinks)
                        .ToList();
            }
            return links;
        }

        public static async Task<int> CountTierItemsAsync(int tier)
        {
            using (var context = new CrawlerContext())
            {
                var tierLength = await context.Links.Where(link => link.Tier == tier).CountAsync();
                return tierLength;
            }
        }

        public static async Task<int> CountAllLinksAsync()
        {
            using (var context = new CrawlerContext())
            {
                var numberOfLinks = await context.Links.CountAsync();
                return numberOfLinks;
            }
        }

        public static async Task SaveFirstLinkAsync(string url)
        {
            var link = new DbLink
            {
                Url = url,
                Domain = LinkService.GetDomain(url),
                IsInternal = true,
                Tier = 0
            };

            using (var context = new CrawlerContext())
            {
                context.Database.ExecuteSqlCommand($"TRUNCATE TABLE Links");
                await context.Links.AddAsync(link);
                context.SaveChanges();
            }
        }

        public static void SaveLinksInDb(List<DbLink> listLinks)
        {
            using (var context = new CrawlerContext())
            {
                foreach(DbLink link in listLinks)
                {
                    context.Links.Add(link);
                }
                context.SaveChanges();
            }
        }

        public static void ChangeDbSettings(DbSettings dbSettings)
        {
            string json = JsonConvert.SerializeObject(dbSettings, Formatting.Indented);
            File.WriteAllText($"config.json", json);
        }

        public static string GetConnectionString()
        {
            var dbSettings = GetDbSettings();

            var sb = new StringBuilder();
            sb.Append("Server=");
            sb.Append(dbSettings.ServerAddress);
            sb.Append(";");
            sb.Append("Database=");
            sb.Append(dbSettings.DbName);
            sb.Append(";");
            sb.Append("User=");
            sb.Append(dbSettings.DbUser);
            sb.Append(";");
            sb.Append("Password=");
            sb.Append(dbSettings.DbPassword);
            sb.Append(";");

            return sb.ToString();
        }

        public static string GetMysqlVersion()
        {
            var dbSettings = GetDbSettings();

            return dbSettings.MysqlVersion;
        }

        private static DbSettings GetDbSettings()
        {
            var json = File.ReadAllText($"config.json");
            var dbSettings = JsonConvert.DeserializeObject<DbSettings>(json);

            return dbSettings;
        }
    }
}
