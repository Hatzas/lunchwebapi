using Lunch.DataAccessLayer.Repositories;
using Lunch.Logging;
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
        [Route("api/usermenu/list")]
        public HttpResponseMessage GetUserMenuList(DateTime startDate, DateTime endDate)
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();
                var userMenus = loggingUnitOfWork.UserMenuRepository.GetUserMenusDetailsByInterval(startDate, endDate);

                return Request.CreateResponse(userMenus);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/usermenu/menulist Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }


        [HttpGet]
        [Route("api/usermenu")]
        public HttpResponseMessage Get(DateTime startDate, DateTime endDate)
        {
            try
            {
                var culture = new System.Globalization.CultureInfo("ro-RO");
                var loggingUnitOfWork = new LunchUnitOfWork();
                var userMenusList = loggingUnitOfWork.UserMenuRepository.GetUserMenusDetailsByInterval(startDate, endDate);

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
                        DishStatistics = menuDetails.DishStatistics.Select(s => new DishStatsModel { Rating = s.Rating, RatingCount = s.RatingCount }).ToList()
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

        // GET: api/UserMenu/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/UserMenu
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/UserMenu/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserMenu/5
        public void Delete(int id)
        {
        }

        public static byte[] StringToByteArray(string hex)
        {

            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
