using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class StorageViewModel : ObservableObject
    {
        private readonly NutritionContext _context;

        [ObservableProperty]
        private ObservableCollection<StorageItem> _storageItems = new();

        public StorageViewModel()
        {
            _context = new NutritionContext();
            LoadStorageItems(); // This will still work as it's now public
        }

        public void LoadStorageItems()
        {
            var items = _context.StorageItems.ToList();
            StorageItems.Clear();
            foreach (StorageItem item in items)
            {
                StorageItems.Add(item);
            }
        }

        [RelayCommand]
        private async Task DeleteStorageItem(StorageItem item)
        {
            bool confirm = await Shell.Current.DisplayAlert("Delete Item", $"Delete {item.Name}?", "Yes", "No");
            if (confirm)
            {
                _context.StorageItems.Remove(item);
                await _context.SaveChangesAsync();
                StorageItems.Remove(item);
            }
        }
    }
}
