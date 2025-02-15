using FoodGenerator.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microcharts;
using NutritionApp.Data.Data;
using NutritionApp.Data.Models;
using System.Collections.ObjectModel;

namespace FoodGenerator
{
    public partial class MainPage : ContentPage
    {
        private readonly NutritionContext _context;
        public ObservableCollection<FoodItem> FoodItems { get; } = new();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
            _context = new NutritionContext();

            // Load food items when the page appears
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

            Console.WriteLine($"Loaded {items.Count} food items from the database.");

            FoodItems.Clear();
            foreach (var item in items)
            {
                FoodItems.Add(item);
            }

            Console.WriteLine($"FoodItems count: {FoodItems.Count}");
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            // Navigate to the AddEditPage to add a new food item
            await Navigation.PushModalAsync(new AddEditPage());
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            // Get the food item to delete from the CommandParameter
            var button = (Button)sender;
            var foodItem = (FoodItem)button.CommandParameter;

            // Confirm deletion with the user
            bool confirm = await DisplayAlert("Delete Food", $"Are you sure you want to delete {foodItem.Name}?", "Yes", "No");
            if (confirm)
            {
                // Remove the item from the database
                _context.FoodItems.Remove(foodItem);
                await _context.SaveChangesAsync();

                // Remove the item from the ObservableCollection
                FoodItems.Remove(foodItem);
            }
        }

        // Unsubscribe from MessagingCenter to avoid memory leaks
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<AddEditPage>(this, "RefreshFoodList");
        }
        private async void OnStorageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StoragePage());
        }
    }
}