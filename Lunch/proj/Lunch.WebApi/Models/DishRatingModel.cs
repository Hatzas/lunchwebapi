using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch.WebApi.Models
{
    public class DishRatingModel
    {
        public int DishId { get; set; }

        public string UserId { get; set; }

        public string Rating { get; set; }
    }
}