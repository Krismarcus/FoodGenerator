using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionApp.Data.Models
{
    [Owned]
    public class Proteins
    {
        public decimal Total { get; set; }

        [NotMapped] // This tells EF Core to ignore this property
        public Dictionary<string, decimal> AminoAcids { get; set; } = new();
    }
}
