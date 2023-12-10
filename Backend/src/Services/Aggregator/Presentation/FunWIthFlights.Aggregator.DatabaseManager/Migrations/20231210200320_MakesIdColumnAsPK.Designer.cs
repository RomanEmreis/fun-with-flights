﻿// <auto-generated />
using FunWithFlights.Aggregator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FunWIthFlights.Aggregator.DatabaseManager.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20231210200320_MakesIdColumnAsPK")]
    partial class MakesIdColumnAsPK
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FunWithFlights.Aggregator.Domain.Entities.FlightRoute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Airline")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CodeShare")
                        .HasColumnType("text");

                    b.Property<string>("DataSourceName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DestinationAirport")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Equipment")
                        .HasColumnType("text");

                    b.Property<string>("SourceAirport")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Stops")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SourceAirport", "DestinationAirport");

                    b.ToTable("Routes");
                });
#pragma warning restore 612, 618
        }
    }
}
