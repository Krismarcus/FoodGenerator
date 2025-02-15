using NutritionApp.Data.Models;
using NutritionApp.Data.Services;

namespace FoodGenerator;

public partial class AddEditPage : ContentPage
{
    private readonly DatabaseService _dbService = new();
    private FoodItem _foodItem;

    public AddEditPage(FoodItem item = null)
    {
        InitializeComponent();

        // Initialize the FoodItem with nested objects
        _foodItem = item ?? new FoodItem
        {
            Fats = new Fats(),
            Proteins = new Proteins(),
            Carbohydrates = new Carbohydrates(),
            Calories = 0,
            Weight = 100
        };

        // Set the BindingContext to the FoodItem
        BindingContext = _foodItem;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            Console.WriteLine($"Saving FoodItem: {_foodItem.Name}");

            if (_foodItem.Id == 0)
            {
                Console.WriteLine("Adding new FoodItem");
                await _dbService.AddFoodItemAsync(_foodItem);
            }
            else
            {
                Console.WriteLine("Updating existing FoodItem");
                await _dbService.UpdateFoodItemAsync(_foodItem);
            }

            // Notify MainViewModel to refresh the data
            MessagingCenter.Send(this, "RefreshFoodList");

            // Navigate back to the previous page
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving FoodItem: {ex.Message}");
            await DisplayAlert("Error", "Failed to save the food item. Please try again.", "OK");
        }
    }
}