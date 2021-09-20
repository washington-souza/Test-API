using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Test.API.MVC.Models;

namespace Test.API.Persistences.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Movie>().ToTable("Movies");
            builder.Entity<Movie>().HasKey(k => k.Id);
            builder.Entity<Movie>().Property(k => k.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Movie>().Property(k => k.Year).IsRequired();
            builder.Entity<Movie>().Property(k => k.Title).IsRequired();
            builder.Entity<Movie>().Property(k => k.Studio).IsRequired();
            builder.Entity<Movie>().Property(k => k.Producer).IsRequired();
            builder.Entity<Movie>().Property(k => k.Winner);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
                MissingFieldFound = null,
                HeaderValidated = null,
                Delimiter = ";",
                BadDataFound = null
            };
            using (var reader = new StreamReader("movielist.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<MovieMap>();
                var records = csv.GetRecords<Movie>();

                int count = 1;
                foreach (var record in records)
                {
                    builder.Entity<Movie>().HasData(
                        new Movie { Id = count++, Year = record.Year, Title = record.Title, Studio = record.Studio, Producer = record.Producer, Winner = record.Winner }
                    );
                }
            }
        }
    }
}