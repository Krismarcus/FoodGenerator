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
        private ObservableCollection<FoodRecipe> _availableRecipes = new();

        [RelayCommand]
        private async Task LoadData()
        {
            AvailableRecipes.Clear();

            using (var context = new NutritionContext())
            {
                var recipes = await context.FoodRecipes
                    .Include(r => r.FoodRecipeItems)
                    .ThenInclude(fri => fri.FoodItem)
                    .ToListAsync();

                var storageItems = await context.StorageItems.ToListAsync();

                foreach (var recipe in recipes)
                {
                    bool canMake = true;
                    foreach (var item in recipe.FoodRecipeItems)
                    {
                        var storageItem = storageItems.FirstOrDefault(s => s.Name == item.FoodItem.Name);
                        if (storageItem == null || storageItem.Weight < item.Weight)
                        {
                            canMake = false;
                            break;
                        }
                    }
                    if (canMake)
                    {
                        AvailableRecipes.Add(recipe);
                    }
                }
            }
        }
    }
}
