using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionApp.Data.Models
{
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Fats Fats { get; set; } = new();
        public Proteins Proteins { get; set; } = new();
        public Carbohydrates Carbohydrates { get; set; } = new();
    }
}
