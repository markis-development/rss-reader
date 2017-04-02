using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using rss_dotnet_api.Models;

namespace rss_dotnet_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170401222947_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("rss_dotnet_api.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Link");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("rss_dotnet_api.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ChannelId");

                    b.Property<string>("Description");

                    b.Property<string>("Link");

                    b.Property<string>("PubDate");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("rss_dotnet_api.Models.Item", b =>
                {
                    b.HasOne("rss_dotnet_api.Models.Channel")
                        .WithMany("ItemList")
                        .HasForeignKey("ChannelId");
                });
        }
    }
}
