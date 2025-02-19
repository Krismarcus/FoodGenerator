using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionApp.Data.Models
{
    public class FoodRecipe
    {
        public int Id { get; set; }
        public string RecipeName { get; set; }
        public List<FoodRecipeItem> FoodRecipeItems { get; set; } = new();
        [Column(TypeName = "DOUBLE")]
        [DefaultValue(0.0)]
        public double Calories => FoodRecipeItems?.Sum(fri =>
    (fri.FoodItem?.Calories ?? 0) * (fri.Weight) / 100) ?? 0;

    }
}
