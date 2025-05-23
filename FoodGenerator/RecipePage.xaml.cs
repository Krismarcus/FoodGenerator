using FoodGenerator.ViewModels;

namespace FoodGenerator;

public partial class RecipePage : ContentPage
{
    public RecipePage()
    {
        InitializeComponent();
        BindingContext = new RecipesViewModel();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AddEditRecipePage());
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is RecipesViewModel viewModel)
        {
            await viewModel.LoadRecipesItemsAsync();
        }
    }
}
