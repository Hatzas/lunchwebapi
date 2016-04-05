using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch.WebApi.Models
{
    public class DeleteMenuModel
    {
        public DateTime StartDate{ get; set; }

        public DateTime EndDate { get; set; }
    }
}