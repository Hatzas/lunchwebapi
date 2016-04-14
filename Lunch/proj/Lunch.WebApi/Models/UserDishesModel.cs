using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch.WebApi.Models
{
    public class UserDishesModel : DishesModel
    {
        public List<DishStatsModel> DishStatistics { get; set; }
        public int? SelectionCount { get; set; }

        public bool Selected { get; set; }
    }
}