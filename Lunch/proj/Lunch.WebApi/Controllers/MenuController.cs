using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunch.DataAccessLayer.Repositories;
using Lunch.Model.Extended;
using System.Threading.Tasks;
using Lunch.Logging;
using Lunch.WebApi.Models;
using Lunch.Model;

namespace Lunch.WebApi.Controllers
{
    public class MenuController : ApiController
    {
        //GET api/menu/list
        [HttpGet]
        [Route("api/menu/list")]
        public HttpResponseMessage GetMenuList(DateTime startDate, DateTime endDate)
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();
                var menuList = loggingUnitOfWork.MenuRepository.GetMenusDetailsByStartDateAndEndDate(startDate, endDate);

                return Request.CreateResponse(menuList);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/menu/menulist Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }


        // GET api/menu
        public HttpResponseMessage Get(DateTime startDate, DateTime endDate)
        {
            try
            {
                var culture = new System.Globalization.CultureInfo("ro-RO");
                var loggingUnitOfWork = new LunchUnitOfWork();
                var menuDetailsList = loggingUnitOfWork.MenuRepository.GetMenusDetailsByStartDateAndEndDate(startDate, endDate);

                var menuList = new List<MenuModel>();

                foreach (var menuDetails in menuDetailsList)
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

                    dayMenu.Dishes.Add(new DishesModel
                    {
                        Id = menuDetails.Dish.Id,
                        Name = menuDetails.Dish.Name,
                        Description = menuDetails.Dish.Description,
                        DishPicture = new DishPictureModel { Id = menuDetails.Dish.DishPicture.Id, Thumbnail = menuDetails.Dish.DishPicture.Thumbnail.ToString() },
                        Type = menuDetails.Dish.Type,
                        Serial = menuDetails.Serial,
                    });
                }


                return Request.CreateResponse(menuList);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/menu Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }


        // POST api/menu
        [HttpPost]
        [HttpPut]
        [Route("api/menu")]
        public HttpResponseMessage UpsertMenu(List<AddMenuModeltem> menuList)
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();

                var dateList = new List<DateTime>();
                foreach (var menu in menuList)
                    dateList.Add(menu.Date);

                var updatedMenuList = loggingUnitOfWork.MenuRepository.GetMenuListByDates(dateList);


                foreach (var menu in menuList)
                {
                    var updatedMenu = updatedMenuList.Where(m => m.Date.Value == menu.Date).FirstOrDefault();
                    if (updatedMenu != null)
                    {

                        updatedMenu.DishId = menu.DishId;
                        updatedMenu.DishCategoryId = menu.DishCategoryId;
                        updatedMenu.Serial = menu.Serial;

                        loggingUnitOfWork.MenuRepository.Upsert(updatedMenu);
                    }
                    else
                    {
                        var newMenu = new Menu
                        {
                            DishId = menu.DishId,
                            DishCategoryId = menu.DishCategoryId,
                            Date = menu.Date,
                            Serial = menu.Serial,
                        };

                        loggingUnitOfWork.MenuRepository.Upsert(newMenu);
                    }
                }

                loggingUnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/menu UpsertMenu: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // DELETE api/menu/delete
        [Route("api/menu/delete")]
        [HttpPost]
        public HttpResponseMessage Delete(DeleteMenuModel model)
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();

                var menuList = loggingUnitOfWork.MenuRepository.GetMenusByStartDateEndDate(model.StartDate, model.EndDate);
                foreach (var menu in menuList)
                {
                    loggingUnitOfWork.MenuRepository.DeleteEntity(menu);
                }
                loggingUnitOfWork.Save();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/menu/delete Post: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
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
