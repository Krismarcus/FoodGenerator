using FoodGenerator.ViewModels;

namespace FoodGenerator;

public partial class AvailableRecipesPage : ContentPage
{
    public AvailableRecipesPage()
    {
        InitializeComponent();
        BindingContext = new AvailableRecipesViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AvailableRecipesViewModel viewModel)
        {
            await viewModel.LoadDataCommand.ExecuteAsync(null);
        }
    }
}