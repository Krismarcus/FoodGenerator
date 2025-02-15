namespace FoodGenerator
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("StoragePage", typeof(StoragePage));
        }
    }
}
