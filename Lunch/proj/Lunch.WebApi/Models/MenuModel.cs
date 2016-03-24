using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch.WebApi.Models
{
    public class MenuModel
    {
        public MenuModel()
        {
            this.Dishes = new List<DishesModel>();
        }


        public DateTime? Date { get; set; }

        public string Day { get; set; }

        public List<DishesModel> Dishes { get; set; }
    }
}