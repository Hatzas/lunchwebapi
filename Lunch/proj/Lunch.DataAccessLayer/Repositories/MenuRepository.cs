using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunch.DataAccess;
using Lunch.Model;
using Lunch.Model.Extended;

namespace Lunch.DataAccessLayer.Repositories
{
    public class MenuRepository: RepositoryBase<Menu>
    {
        #region Constructor
        internal MenuRepository(Entities context)
            : base(context)
        {
        }
        #endregion

        #region Public methods
        public void Upsert(Menu entity)
        {
            if (entity.Id == default(int))
            {
                this.InsertEntity(entity);
            }
            else
            {
                this.UpdateEntity(entity);
            }
        }

        public List<Menu> GetMenusByDate(DateTime date)
        {
            return DbContext.Set<Menu>().Where(m => m.Date == date).ToList();
        }
        public List<Menu> GetMenusByStartDateEndDate(DateTime starDate, DateTime endDate)
        {
            return DbContext.Set<Menu>().Where(m => m.Date >= starDate && m.Date >= endDate).ToList();
        }

        public void DeleteMenuById(Menu menu)
        {
            DbContext.Set<Menu>().Remove(menu);            
        }

        public List<MenuDetails> GetMenusDetailsByStartDateAndEndDate(DateTime starDate, DateTime endDate)
        {
            var query = (from menu in this.DbContext.Menus
                         where menu.Date>=starDate && menu.Date<= endDate
                         select new MenuDetails
                         {
                             Id = menu.Id,
                             Date = menu.Date,
                             Serial = menu.Serial,
                             Dish = new DishDetails()
                             {
                                 Id = menu.Dish.Id,
                                 Name = menu.Dish.Name,
                                 Description = menu.Dish.Description,
                                 Type = menu.Dish.Type,
                                 DishPicture = new DishPictureDetails()
                                 {
                                     Id= menu.Dish.DishPicture.Id,
                                     Thumbnail= menu.Dish.DishPicture.Thumbnail,
                                 }
                             },
                             DishCategory = new DishCategoryDetails()
                             {
                                 Id = menu.DishCategory.Id,
                                 Name = menu.DishCategory.Name,
                             },
                         });

            return query.ToList();
        }     
        #endregion

        #region Private methods
      
        #endregion
    }
}
