using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using NutritionApp.Data.Data;
using NutritionApp.Data.Models;
using System.Collections.ObjectModel;

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
}