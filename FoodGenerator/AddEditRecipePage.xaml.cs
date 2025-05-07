using FoodGenerator.ViewModels;
using NutritionApp.Data.Models;

namespace FoodGenerator;

public partial class AddEditRecipePage : ContentPage
{
    public AddEditRecipePage(FoodRecipe recipe = null)
    {
        InitializeComponent();
        BindingContext = new AddEditRecipeViewModel(recipe);
    }
}