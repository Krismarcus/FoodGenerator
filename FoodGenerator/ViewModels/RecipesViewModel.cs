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
                                             .Include(r => r.FoodItems)
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
            bool confirm = await Shell.Current.DisplayAlert("Delete Item", $"Delete {item.RecipeName}?", "Yes", "No");
            if (confirm)
            {
                try
                {
                    using (var context = new NutritionContext()) // Create a new context
                    {
                        context.FoodRecipes.Remove(item);
                        await context.SaveChangesAsync();
                        FoodRecipes.Remove(item);
                    }
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", $"Failed to delete recipe: {ex.Message}", "OK");
                }
            }
        }
    }
}