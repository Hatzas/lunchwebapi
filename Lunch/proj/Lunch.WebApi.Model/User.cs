//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lunch.WebApi.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.UserMenus = new HashSet<UserMenu>();
            this.Roles = new HashSet<Role>();
        }
    
        public int Id { get; set; }
        public string UserGUID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<UserMenu> UserMenus { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
