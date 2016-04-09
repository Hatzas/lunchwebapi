using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch.WebApi.Models
{
    public class AddMenuModeltem
    {
        public int DishId { get; set; }

        public int DishCategoryId { get; set; }

        public DateTime Date { get; set; }

        public string Serial { get; set; }
    }
}