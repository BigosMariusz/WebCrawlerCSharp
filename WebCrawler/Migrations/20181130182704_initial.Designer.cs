﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebCrawler.Data;

namespace WebCrawler.Migrations
{
    [DbContext(typeof(CrawlerContext))]
    [Migration("20181130182704_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WebCrawler.Data.DbLink", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Domain");

                    b.Property<bool>("IsLocal");

                    b.Property<string>("ParentUrl");

                    b.Property<int>("Tier");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Links");
                });
#pragma warning restore 612, 618
        }
    }
}