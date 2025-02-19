using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionApp.Data.Models
{
    public class FoodRecipeItem
    {
        public int Id { get; set; }

        public int FoodRecipeId { get; set; }
        public FoodRecipe FoodRecipe { get; set; }

        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }

        public double Weight { get; set; }

    }        
}
