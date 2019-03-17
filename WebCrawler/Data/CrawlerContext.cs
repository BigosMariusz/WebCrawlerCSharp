using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebCrawler.Services;

namespace WebCrawler.Data
{
    class CrawlerContext : DbContext
    {
        string connectionString = DbService.GetConnectionString();

        public DbSet<DbLink> Links { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbLink>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}

