using Lunch.DataAccessLayer.Repositories;
using Lunch.Logging;
using Lunch.Model;
using Lunch.Model.Extended;
using Lunch.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lunch.WebApi.Controllers
{
    public class UserMenuController : ApiController
    {
        [HttpGet]
        [Route("api/usermenu")]
        public HttpResponseMessage Get(DateTime startDate, DateTime endDate, string user)
        {
            try
            {
                var culture = new System.Globalization.CultureInfo("ro-RO");
                var lunchUnitOfWork = new LunchUnitOfWork();


                var userMenusList = new List<MenuDetails>();
                if (!string.IsNullOrEmpty(user))
                {
                    userMenusList = lunchUnitOfWork.UserMenuRepository.GetUserMenusDetailsByInterval(startDate, endDate, user);
                }
                else
                {
                    userMenusList = lunchUnitOfWork.UserMenuRepository.GetUserMenusDetailsByInterval2(startDate, endDate);
                }


                var menuList = new List<MenuModel>();

                foreach (var menuDetails in userMenusList)
                {
                    var dayMenu = menuList.FirstOrDefault(m => m.Date.HasValue && menuDetails.Date.HasValue && m.Date.Value.Date == menuDetails.Date.Value.Date);

                    if (dayMenu == null)
                    {
                        dayMenu = new MenuModel
                        {
                            Date = menuDetails.Date.Value,
                            Day = culture.DateTimeFormat.GetDayName(menuDetails.Date.Value.DayOfWeek),
                        };
                        menuList.Add(dayMenu);
                    }

                    dayMenu.Dishes.Add(new UserDishesModel
                    {
                        Id = menuDetails.Dish.Id,
                        Name = menuDetails.Dish.Name,
                        Description = menuDetails.Dish.Description,
                        DishPicture = new DishPictureModel { Id = menuDetails.Dish.DishPicture.Id, Thumbnail = Convert.ToBase64String(menuDetails.Dish.DishPicture.Thumbnail) },
                        Type = menuDetails.Dish.Type,
                        Serial = menuDetails.Serial,
                        Category = menuDetails.DishCategory.Id.ToString(),
                        DishStatistics = menuDetails.DishStatistics.Select(s => new DishStatsModel { Rating = s.Rating, RatingCount = s.RatingCount }).ToList(),
                        SelectionCount = menuDetails.SelectionCount.HasValue ? menuDetails.SelectionCount : 0,
                        Selected = menuDetails.Selected
                    });
                }


                return Request.CreateResponse(menuList);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/usermenu Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }


        [HttpPost]
        [HttpPut]
        [Route("api/usermenu")]
        public HttpResponseMessage UpsertUserMenu(UserMenuModel model)
        {
            try
            {
                var menuDateList = model.UserMenus.Select(m => m.Date).ToList();

                var lunchUnitOfWork = new LunchUnitOfWork();
                
                var user = lunchUnitOfWork.UserRepository.GetUserByName(model.UserId);


                var userMenuList = lunchUnitOfWork.UserMenuRepository.GetUserMenuListByDates(menuDateList);
                foreach (var userMenu in userMenuList)
                {
                    lunchUnitOfWork.UserMenuRepository.DeleteEntity(userMenu);
                }


                foreach (var item in model.UserMenus)
                {
                    var menu = lunchUnitOfWork.MenuRepository.GetMenuByDateAndDish(item.Date, item.DishId);

                    var userMenu = new UserMenu
                    {
                        Date = item.Date,
                        MenuId = menu.DishId,
                        UserId = user.Id,
                    };
                    lunchUnitOfWork.UserMenuRepository.Upsert(userMenu);
                }

                
                lunchUnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/usermenu Post/Put: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }


        [HttpDelete]
        [Route("api/usermenu")]
        public HttpResponseMessage Delete(DeleteUserMenuModel model)
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();

                var user = lunchUnitOfWork.UserRepository.GetUserByName(model.UserId);
                var userMenuList = lunchUnitOfWork.UserMenuRepository.GetUserMenusByInterval(model.StartDate, model.EndDate, user.Id);

                foreach (var userMenu in userMenuList)
                {
                    lunchUnitOfWork.UserMenuRepository.DeleteEntity(userMenu);
                }


                lunchUnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/usermenu Delete: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }


        #region Private
        private static byte[] StringToByteArray(string hex)
        {

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        #endregion
    }
}
