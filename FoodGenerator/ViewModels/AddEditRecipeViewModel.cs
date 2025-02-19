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
    public partial class AddEditRecipeViewModel : ObservableObject
    {
        private readonly NutritionContext _context = new();

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private double _weight; // ✅ New property to store weight in grams

        [ObservableProperty]
        private ObservableCollection<string> _suggestions = new();

        [ObservableProperty]
        private bool _showSuggestions;

        [ObservableProperty]
        private ObservableCollection<FoodRecipeItem> _selectedFoodRecipeItems = new(); // ✅ Updated from FoodItem

        [ObservableProperty]
        private string _recipeName;

        /// <summary>
        /// Save the new recipe with selected food items and grams
        /// </summary>
        [RelayCommand]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(RecipeName) || SelectedFoodRecipeItems.Count == 0) return;

            try
            {
                var newRecipe = new FoodRecipe
                {
                    RecipeName = RecipeName,
                    FoodRecipeItems = new List<FoodRecipeItem>()
                };

                foreach (var item in SelectedFoodRecipeItems)
                {
                    _context.Attach(item.FoodItem); // ✅ Ensure existing FoodItem is tracked
                    newRecipe.FoodRecipeItems.Add(item);
                }

                _context.FoodRecipes.Add(newRecipe);
                await _context.SaveChangesAsync();

                MessagingCenter.Send(this, "RefreshRecipeList");
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to save recipe: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Select a food item suggestion
        /// </summary>
        [RelayCommand]
        private void SelectSuggestion(string suggestion)
        {
            SearchText = suggestion;
            ShowSuggestions = false;
        }

        /// <summary>
        /// Add a food item with grams to the recipe
        /// </summary>
        [RelayCommand]
        private void AddFoodItem()
        {
            if (string.IsNullOrWhiteSpace(SearchText) || Weight <= 0) return;

            var foodItem = _context.FoodItems.FirstOrDefault(f => f.Name == SearchText);
            if (foodItem != null)
            {
                // ✅ Create a new FoodRecipeItem with grams
                var recipeItem = new FoodRecipeItem
                {
                    FoodItem = foodItem,
                    Weight = _weight
                };

                SelectedFoodRecipeItems.Add(recipeItem);
                SearchText = string.Empty; // ✅ Clear search text
                Weight = 0; // ✅ Reset grams input
            }
        }
        
        [RelayCommand]
        private void RemoveFoodItem(FoodRecipeItem item)
        {
            SelectedFoodRecipeItems.Remove(item);
        }

        partial void OnSearchTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                LoadSuggestions(value);
            }
            else
            {
                ShowSuggestions = false;
            }
        }

        /// <summary>
        /// Load food item suggestions based on search input
        /// </summary>
        private void LoadSuggestions(string searchText)
        {
            var foodNames = _context.FoodItems
                .Where(f => f.Name.Contains(searchText))
                .Select(f => f.Name)
                .Distinct()
                .Take(5)
                .ToList();

            Suggestions = new ObservableCollection<string>(foodNames);
            ShowSuggestions = Suggestions.Count > 0;
        }
    }
}