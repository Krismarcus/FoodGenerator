using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using NutritionApp.Data.Data;
using NutritionApp.Data.Models;
using System.Collections.ObjectModel;

namespace FoodGenerator.ViewModels
{
    public partial class AvailableRecipesViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<RecipeStatus> _availableRecipes = new();

        [ObservableProperty]
        private ObservableCollection<RecipeStatus> _missingRecipes = new();

        public class RecipeStatus
        {
            public FoodRecipe Recipe { get; set; }
            public List<MissingFoodItem> MissingItems { get; set; } = new();
        }

        [RelayCommand]
        private async Task AddIngredient(MissingFoodItem missingItem)
        {
            if (string.IsNullOrWhiteSpace(missingItem.AmountToAdd)) return;
            if (!double.TryParse(missingItem.AmountToAdd, out double amount) || amount <= 0)
            {
                await Shell.Current.DisplayAlert("Error", "Please enter a valid positive number", "OK");
                return;
            }

            using var context = new NutritionContext();
            var storageItem = await context.StorageItems
                .FirstOrDefaultAsync(s => s.Name == missingItem.ItemName);

            if (storageItem == null)
            {
                storageItem = new StorageItem
                {
                    Name = missingItem.ItemName,
                    Weight = amount
                };
                await context.StorageItems.AddAsync(storageItem);
            }
            else
            {
                storageItem.Weight += amount;
            }

            await context.SaveChangesAsync();
            missingItem.AmountToAdd = string.Empty;
            await LoadDataCommand.ExecuteAsync(null);
        }

        [RelayCommand]
        private async Task LoadData()
        {
            AvailableRecipes.Clear();
            MissingRecipes.Clear();

            using var context = new NutritionContext();
            
            var recipes = await context.FoodRecipes
                .Include(r => r.FoodRecipeItems)
                .ThenInclude(fri => fri.FoodItem)
                .ToListAsync();

            var storageItems = await context.StorageItems.ToListAsync();

            foreach (var recipe in recipes)
            {
                var missingItems = new List<MissingFoodItem>();
                bool canMake = true;

                foreach (var recipeItem in recipe.FoodRecipeItems)
                {
                    var storageItem = storageItems.Find(s => s.Name == recipeItem.FoodItem.Name);
                    var available = storageItem?.Weight ?? 0;
                    var required = recipeItem.Weight;

                    if (available < required)
                    {
                        canMake = false;
                        missingItems.Add(new MissingFoodItem
                        {
                            ItemName = recipeItem.FoodItem.Name,
                            Required = required,
                            Available = available
                        });
                    }
                }

                var status = new RecipeStatus
                {
                    Recipe = recipe,
                    MissingItems = missingItems
                };

                if (canMake)
                {
                    AvailableRecipes.Add(status);
                }
                else
                {
                    MissingRecipes.Add(status);
                }
            }
        }
    }
}
