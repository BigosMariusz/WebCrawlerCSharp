using System;
using System.Collections.Generic;
using System.Text;

namespace WebCrawler.Data
{
    public class DbLink
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Guid ParentId { get; set; }
        public string Domain { get; set; }
        public bool IsInternal { get; set; }
        public int Tier { get; set; }
    }
}
