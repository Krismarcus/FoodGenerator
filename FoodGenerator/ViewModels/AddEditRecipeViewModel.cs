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
        private double _weight;

        [ObservableProperty]
        private ObservableCollection<string> _suggestions = new();

        [ObservableProperty]
        private bool _showSuggestions;

        [ObservableProperty]
        private ObservableCollection<FoodItem> _selectedFoodItems = new();

        [ObservableProperty]
        private string _recipeName;

        [RelayCommand]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(RecipeName) || SelectedFoodItems.Count == 0) return;

            try
            {
                var newRecipe = new FoodRecipe
                {
                    RecipeName = RecipeName,
                    FoodItems = new List<FoodItem>()
                };

                newRecipe.FoodItems.AddRange(SelectedFoodItems);
                _context.FoodRecipes.Add(newRecipe);

                await _context.SaveChangesAsync();
                MessagingCenter.Send(this, "RefreshRecipeList");
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine($"DB Update Error: {dbEx}");
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {dbEx.InnerException.Message}");
                }

                await Shell.Current.DisplayAlert("Error", $"Failed to save recipe: {dbEx.InnerException?.Message ?? dbEx.Message}", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving recipe: {ex}");
                await Shell.Current.DisplayAlert("Error", $"Failed to save recipe: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private void SelectSuggestion(string suggestion)
        {
            SearchText = suggestion;
            ShowSuggestions = false;
        }

        [RelayCommand]
        private void AddFoodItem()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return;

            var foodItem = _context.FoodItems.FirstOrDefault(f => f.Name == SearchText);
            if (foodItem != null)
            {
                SelectedFoodItems.Add(foodItem);
                SearchText = string.Empty; // Clear the search text
            }
        }

        [RelayCommand]
        private void RemoveFoodItem(FoodItem item)
        {
            SelectedFoodItems.Remove(item);
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