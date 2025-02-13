using NutritionApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionApp.Data.Data
{
    public class NutritionContext : DbContext
    {
        public DbSet<FoodItem> FoodItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=foodgenerator-astrodaiva-test.j.aivencloud.com;" +
                                  "Database=defaultdb;" +
                                  "Port=13820;" +
                                  "User=avnadmin;" +
                                  "Password=AVNS_a2WiRa0GI5oY6iRbpcA;" +
                                  "SslMode=Required;";

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.OwnsOne(e => e.Fats);
                entity.OwnsOne(e => e.Proteins);
                entity.OwnsOne(e => e.Carbohydrates);
            });
        }

    }
}
