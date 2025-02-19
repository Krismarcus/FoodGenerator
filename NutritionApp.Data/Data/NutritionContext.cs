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
        public DbSet<StorageItem> StorageItems { get; set; }
        public DbSet<FoodRecipe> FoodRecipes { get; set; }
        public DbSet<FoodRecipeItem> FoodRecipeItems { get; set; }

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
            // Configure FoodItem
            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.OwnsOne(e => e.Fats);
                entity.OwnsOne(e => e.Proteins);
                entity.OwnsOne(e => e.Carbohydrates);
            });

            // Configure StorageItem
            modelBuilder.Entity<StorageItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            // Configure FoodRecipe
            modelBuilder.Entity<FoodRecipe>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RecipeName).IsRequired();
            });

            // Configure FoodRecipeItem (Join Table with Grams)
            modelBuilder.Entity<FoodRecipeItem>(entity =>
            {
                entity.HasKey(fri => fri.Id); // Primary Key

                entity.HasOne(fri => fri.FoodRecipe)
                      .WithMany(fr => fr.FoodRecipeItems)
                      .HasForeignKey(fri => fri.FoodRecipeId)
                      .OnDelete(DeleteBehavior.Cascade); // If a recipe is deleted, remove its ingredients

                entity.HasOne(fri => fri.FoodItem)
                      .WithMany()
                      .HasForeignKey(fri => fri.FoodItemId)
                      .OnDelete(DeleteBehavior.Restrict); // Don't delete FoodItem when removing a recipe
            });
        }
    }
}