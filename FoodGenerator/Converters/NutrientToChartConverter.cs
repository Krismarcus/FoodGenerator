using Microcharts;
using NutritionApp.Data.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGenerator.Converters
{
    public class NutrientToChartConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FoodItem foodItem)
            {
                var entries = new[]
                {
                new ChartEntry((float)foodItem.Fats.Total) // Mango Tango
                {
                    Color = SKColor.Parse("#FF9F43"),
                    ValueLabel = $"{foodItem.Fats.Total:F1}g",
                    Label = "Fats"
                },
                new ChartEntry((float)foodItem.Proteins.Total) // Lavender Purple
                {
                    Color = SKColor.Parse("#6C5CE7"),
                    ValueLabel = $"{foodItem.Proteins.Total:F1}g",
                    Label = "Protein"
                },
                new ChartEntry((float)foodItem.Carbohydrates.Total) // Mint Green
                {
                    Color = SKColor.Parse("#00B894"),
                    ValueLabel = $"{foodItem.Carbohydrates.Total:F1}g",
                    Label = "Carbs"
                }
            };

                return new DonutChart
                {
                    Entries = entries,
                    HoleRadius = 0.8f,
                    LabelTextSize = 24,
                    BackgroundColor = SKColors.Transparent,
                    LabelMode = LabelMode.None,
                    AnimationDuration = TimeSpan.FromMilliseconds(500),
                };
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

