﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NutritionApp.Data.Data;

#nullable disable

namespace NutritionApp.Data.Migrations
{
    [DbContext(typeof(NutritionContext))]
    partial class NutritionContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("NutritionApp.Data.Models.FoodItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Calories")
                        .HasColumnType("double");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Weight")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("NutritionApp.Data.Models.FoodRecipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RecipeName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("FoodRecipes");
                });

            modelBuilder.Entity("NutritionApp.Data.Models.FoodRecipeItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FoodItemId")
                        .HasColumnType("int");

                    b.Property<int>("FoodRecipeId")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("FoodItemId");

                    b.HasIndex("FoodRecipeId");

                    b.ToTable("FoodRecipeItems");
                });

            modelBuilder.Entity("NutritionApp.Data.Models.StorageItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Weight")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("StorageItems");
                });

            modelBuilder.Entity("NutritionApp.Data.Models.FoodItem", b =>
                {
                    b.OwnsOne("NutritionApp.Data.Models.Carbohydrates", "Carbohydrates", b1 =>
                        {
                            b1.Property<int>("FoodItemId")
                                .HasColumnType("int");

                            b1.Property<decimal>("ComplexCarbs")
                                .HasColumnType("decimal(65,30)");

                            b1.Property<decimal>("Fiber")
                                .HasColumnType("decimal(65,30)");

                            b1.Property<decimal>("Sugar")
                                .HasColumnType("decimal(65,30)");

                            b1.Property<decimal>("Total")
                                .HasColumnType("decimal(65,30)");

                            b1.HasKey("FoodItemId");

                            b1.ToTable("FoodItems");

                            b1.WithOwner()
                                .HasForeignKey("FoodItemId");
                        });

                    b.OwnsOne("NutritionApp.Data.Models.Fats", "Fats", b1 =>
                        {
                            b1.Property<int>("FoodItemId")
                                .HasColumnType("int");

                            b1.Property<decimal>("Omega3")
                                .HasColumnType("decimal(65,30)");

                            b1.Property<decimal>("Omega6")
                                .HasColumnType("decimal(65,30)");

                            b1.Property<decimal>("Total")
                                .HasColumnType("decimal(65,30)");

                            b1.Property<decimal>("TransFat")
                                .HasColumnType("decimal(65,30)");

                            b1.HasKey("FoodItemId");

                            b1.ToTable("FoodItems");

                            b1.WithOwner()
                                .HasForeignKey("FoodItemId");
                        });

                    b.OwnsOne("NutritionApp.Data.Models.Proteins", "Proteins", b1 =>
                        {
                            b1.Property<int>("FoodItemId")
                                .HasColumnType("int");

                            b1.Property<decimal>("Total")
                                .HasColumnType("decimal(65,30)");

                            b1.HasKey("FoodItemId");

                            b1.ToTable("FoodItems");

                            b1.WithOwner()
                                .HasForeignKey("FoodItemId");
                        });

                    b.Navigation("Carbohydrates")
                        .IsRequired();

                    b.Navigation("Fats")
                        .IsRequired();

                    b.Navigation("Proteins")
                        .IsRequired();
                });

            modelBuilder.Entity("NutritionApp.Data.Models.FoodRecipeItem", b =>
                {
                    b.HasOne("NutritionApp.Data.Models.FoodItem", "FoodItem")
                        .WithMany()
                        .HasForeignKey("FoodItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NutritionApp.Data.Models.FoodRecipe", "FoodRecipe")
                        .WithMany("FoodRecipeItems")
                        .HasForeignKey("FoodRecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodItem");

                    b.Navigation("FoodRecipe");
                });

            modelBuilder.Entity("NutritionApp.Data.Models.FoodRecipe", b =>
                {
                    b.Navigation("FoodRecipeItems");
                });
#pragma warning restore 612, 618
        }
    }
}
