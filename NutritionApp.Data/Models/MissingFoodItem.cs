using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionApp.Data.Models
{
    public partial class MissingFoodItem : ObservableObject
    {
        public string ItemName { get; set; }
        public double Required { get; set; }
        public double Available { get; set; }
        
        [ObservableProperty]
        public string _amountToAdd;
    }
}
