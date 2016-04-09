using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch.WebApi.Models
{
    public class DishPictureModel
    {
        public int Id { get; set; }
        //public byte[] Thumbnail { get; set; }
        
        public string Thumbnail { get; set; }
    }
}