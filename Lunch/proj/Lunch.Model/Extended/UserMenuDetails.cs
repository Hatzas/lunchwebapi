﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunch.Model.Extended
{
    public class UserMenuDetails
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Serial { get; set; }
        public DishDetails Dish { get; set; }
        public DishCategoryDetails DishCategory { get; set; }
        public IEnumerable<DishStatsDetails> DishStatistics { get; set; }
    }
}
