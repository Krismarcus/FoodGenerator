﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionApp.Data.Models
{
    public partial class StorageItem : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ObservableProperty]
        public double weight;
    }
}
