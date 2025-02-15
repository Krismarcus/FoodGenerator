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
using System.Xml.Linq;

namespace FoodGenerator.ViewModels
{
    public partial class AddEditStorageViewModel : ObservableObject
    {
        private readonly NutritionContext _context = new();

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private double _weight;

        [ObservableProperty]
        private ObservableCollection<string> _suggestions = new();

        [ObservableProperty]
        private bool _showSuggestions;

        [RelayCommand]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(SearchText)) return;

            var newItem = new StorageItem
            {
                Name = SearchText,
                Weight = Weight
            };

            _context.StorageItems.Add(newItem);
            await _context.SaveChangesAsync();
            MessagingCenter.Send(this, "RefreshStorageList");
            await Shell.Current.Navigation.PopModalAsync();
        }

        [RelayCommand]
        private void SelectSuggestion(string suggestion)
        {
            SearchText = suggestion;
            ShowSuggestions = false;
        }

        partial void OnSearchTextChanged(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                LoadSuggestions(value);
            }
            else
            {
                ShowSuggestions = false;
            }
        }

        private void LoadSuggestions(string searchText)
        {
            var foodNames = _context.FoodItems
                .Where(f => f.Name.Contains(searchText))
                .Select(f => f.Name)
                .Distinct()
                .Take(5)
                .ToList();

            Suggestions = new ObservableCollection<string>(foodNames);
            ShowSuggestions = Suggestions.Count > 0;
        }
    }
}
