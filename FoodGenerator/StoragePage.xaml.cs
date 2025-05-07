using FoodGenerator.ViewModels;

namespace FoodGenerator;

public partial class StoragePage : ContentPage
{
    public StoragePage()
    {
        InitializeComponent();
        BindingContext = new StorageViewModel();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AddEditStoragePage());
    }

    private async void OnAvailableRecipesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AvailableRecipesPage());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is StorageViewModel viewModel)
        {
            viewModel.LoadStorageItems();
        }
    }
}