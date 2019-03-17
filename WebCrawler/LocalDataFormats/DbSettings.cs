using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler.LocalDataFormats
{
    public class DbSettings
    {
        public string ServerAddress { get; set; }
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
        public string MysqlVersion { get; set; }
    }
}
