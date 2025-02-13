using NutritionApp.Data.Data;
using NutritionApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionApp.Data.Services
{
    public class DatabaseService
    {
        private readonly NutritionContext _context;

        public DatabaseService()
        {
            _context = new NutritionContext();
        }

        public async Task AddFoodItemAsync(FoodItem item)
        {
            _context.FoodItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFoodItemAsync(FoodItem item)
        {
            _context.FoodItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FoodItem>> GetFoodItemsAsync()
        {
            return await _context.FoodItems
                .Include(f => f.Fats)
                .Include(f => f.Proteins)
                .Include(f => f.Carbohydrates)
                .ToListAsync();
        }

        public async Task DeleteFoodItemAsync(int id)
        {
            var item = await _context.FoodItems.FindAsync(id);
            if (item != null)
            {
                _context.FoodItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
