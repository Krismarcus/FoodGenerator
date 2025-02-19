using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionApp.Data.Models
{
    [Owned]
    public class Fats
    {

        public decimal Total { get; set; }
        public decimal Omega3 { get; set; }
        public decimal Omega6 { get; set; }
        public decimal TransFat { get; set; }
    }
}
