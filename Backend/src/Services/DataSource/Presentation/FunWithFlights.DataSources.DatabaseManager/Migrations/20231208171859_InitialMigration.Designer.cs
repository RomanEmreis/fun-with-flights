﻿// <auto-generated />
using FunWithFlights.DataSources.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FunWithFlights.DataSources.DatabaseManager.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20231208171859_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FunWithFlights.DataSources.Domain.Entities.DataSource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DataSources");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Sample Data Source provider",
                            Name = "Provider 1",
                            Url = "https://zretmlbsszmm4i35zrihcflchm0ktwwj.lambda-url.eu-central-1.on.aws/provider/flights1"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Sample Data Source provider",
                            Name = "Provider 2",
                            Url = "https://zretmlbsszmm4i35zrihcflchm0ktwwj.lambda-url.eu-central-1.on.aws/provider/flights2"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
