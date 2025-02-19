using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using NutritionApp.Data.Data;
using NutritionApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGenerator.ViewModels
{
    public partial class RecipesViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<FoodRecipe> _foodRecipes = new();

        public RecipesViewModel()
        {
            
        }

        public async Task LoadRecipesItemsAsync()
        {
            try
            {
                using (var context = new NutritionContext()) // Create a new context
                {
                    var items = await context.FoodRecipes
                                             .Include(r => r.FoodRecipeItems)
                                             .ToListAsync()
                                             .ConfigureAwait(false);

                    FoodRecipes = new ObservableCollection<FoodRecipe>(items);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load recipes: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task DeleteRecipeItem(FoodRecipe item)
        {
            if (item == null) return;

            bool confirm = await Shell.Current.DisplayAlert("Delete Recipe", $"Are you sure you want to delete {item.RecipeName}?", "Yes", "No");
            if (!confirm) return;

            try
            {
                using (var context = new NutritionContext())
                {
                    var recipeToDelete = context.FoodRecipes
                        .Include(r => r.FoodRecipeItems) // ✅ Ensure FoodRecipeItems are loaded
                        .FirstOrDefault(r => r.Id == item.Id);

                    if (recipeToDelete != null)
                    {
                        context.FoodRecipeItems.RemoveRange(recipeToDelete.FoodRecipeItems); // ✅ Remove linked FoodRecipeItems first
                        context.FoodRecipes.Remove(recipeToDelete); // ✅ Then remove the recipe
                        await context.SaveChangesAsync();

                        FoodRecipes.Remove(item); // ✅ Remove from UI collection
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to delete recipe: {ex.Message}", "OK");
            }
        }
    }
}