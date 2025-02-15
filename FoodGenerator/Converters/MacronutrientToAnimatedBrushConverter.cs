using NutritionApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGenerator.Converters
{
    public class MacronutrientToAnimatedBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not FoodItem foodItem)
                return new SolidColorBrush(Colors.Transparent);

            double total = (double)(foodItem.Fats.Total + foodItem.Proteins.Total + foodItem.Carbohydrates.Total);
            if (total == 0) return new SolidColorBrush(Colors.Gray);

            double fatPercent = (double)foodItem.Fats.Total / total;
            double proteinPercent = (double)foodItem.Proteins.Total / total;
            double carbPercent = (double)foodItem.Carbohydrates.Total / total;

            var brush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };

            // Custom colors from your previous converter
            var fatStart = new GradientStop { Color = Color.FromArgb("#FF9F43"), Offset = 0 }; // Orange
            var fatEnd = new GradientStop { Color = Color.FromArgb("#FF9F43"), Offset = 0 };

            var proteinStart = new GradientStop { Color = Color.FromArgb("#6C5CE7"), Offset = 0 }; // Purple
            var proteinEnd = new GradientStop { Color = Color.FromArgb("#6C5CE7"), Offset = 0 };

            var carbStart = new GradientStop { Color = Color.FromArgb("#00B894"), Offset = 0 }; // Mint Green
            var carbEnd = new GradientStop { Color = Color.FromArgb("#00B894"), Offset = 0 };

            brush.GradientStops.Add(fatStart);
            brush.GradientStops.Add(fatEnd);
            brush.GradientStops.Add(proteinStart);
            brush.GradientStops.Add(proteinEnd);
            brush.GradientStops.Add(carbStart);
            brush.GradientStops.Add(carbEnd);

            // 4x Faster Animation
            AnimateGradientStop(fatEnd, fatPercent, 0.1);
            AnimateGradientStop(proteinStart, fatPercent, 0.1);
            AnimateGradientStop(proteinEnd, fatPercent + proteinPercent, 0.15);
            AnimateGradientStop(carbStart, fatPercent + proteinPercent, 0.15);
            AnimateGradientStop(carbEnd, 1, 0.2);

            return brush;
        }

        private async void AnimateGradientStop(GradientStop stop, double targetOffset, double duration)
        {
            double step = 0.02;
            double interval = 5; // Faster animation
            double increment = (targetOffset - stop.Offset) / (duration * 1000 / interval);

            while (Math.Abs(stop.Offset - targetOffset) > 0.01)
            {
                stop.Offset += (float)increment;
                await Task.Delay((int)interval);
            }

            stop.Offset = (float)targetOffset;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
