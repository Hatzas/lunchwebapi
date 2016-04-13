using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch.WebApi.Models
{
    public class UserMenuModel
    {
        public List<UserMenuItem> UserMenus { get; set; }

        public string UserName { get; set; }
    }
}