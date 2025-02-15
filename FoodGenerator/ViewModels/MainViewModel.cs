using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using NutritionApp.Data.Data;
using NutritionApp.Data.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace FoodGenerator.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NutritionContext _context;

    [ObservableProperty]
    private ObservableCollection<FoodItem> _foodItems = new();    

    public MainViewModel()
    {
        _context = new NutritionContext();
        LoadFoodItems();

        // Subscribe to the RefreshFoodList message
        MessagingCenter.Subscribe<AddEditPage>(this, "RefreshFoodList", (sender) =>
        {
            // Refresh the food list
            LoadFoodItems();
        });
    }

    private void LoadFoodItems()
    {
        var items = _context.FoodItems
            .Include(f => f.Fats)
            .Include(f => f.Proteins)
            .Include(f => f.Carbohydrates)            
            .ToList();

        FoodItems.Clear();
        foreach (var item in items)
        {
            FoodItems.Add(item);
        }
    }        

    [RelayCommand]
    private async Task DeleteFood(FoodItem foodItem)
    {
        bool confirm = await Shell.Current.DisplayAlert("Delete Food", $"Are you sure you want to delete {foodItem.Name}?", "Yes", "No");
        if (confirm)
        {
            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();
            FoodItems.Remove(foodItem);
        }
    }

    // Unsubscribe from MessagingCenter to avoid memory leaks
    ~MainViewModel()
    {
        MessagingCenter.Unsubscribe<AddEditPage>(this, "RefreshFoodList");
    }

    [RelayCommand]
    private async Task Import()
    {
        try
        {
            // 1. Read JSON file
            using var stream = await FileSystem.OpenAppPackageFileAsync("FoodItems.json");
            using var reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();

            // 2. Deserialize JSON
            var foodItems = JsonSerializer.Deserialize<List<FoodItem>>(jsonContent);

            // 3. Save to database
            await _context.Database.EnsureCreatedAsync();
            await _context.FoodItems.AddRangeAsync(foodItems);
            await _context.SaveChangesAsync();

            // 4. Refresh UI
            LoadFoodItems();

            await Shell.Current.DisplayAlert("Success",
                $"Imported {foodItems.Count} food items", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error",
                $"Import failed: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteFoodItems()
    {
        try
        {
            // Confirm deletion with the user
            bool confirm = await Shell.Current.DisplayAlert(
                "Delete Food Items",
                "Are you sure you want to delete all food items? This action cannot be undone.",
                "Yes", "No");

            if (confirm)
            {
                // Delete all entries in the FoodItems table
                var allFoodItems = await _context.FoodItems.ToListAsync();
                _context.FoodItems.RemoveRange(allFoodItems);
                await _context.SaveChangesAsync();

                // Notify the user
                await Shell.Current.DisplayAlert("Success", "All food items deleted successfully.", "OK");

                // Refresh the UI (if needed)
                LoadFoodItems(); // Assuming this method refreshes your UI
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to delete food items: {ex.Message}", "OK");
        }
    }
}