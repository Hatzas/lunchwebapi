using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch.WebApi.Models
{
    public class DeleteUserMenuModel
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UserId { get; set; }
    }
}