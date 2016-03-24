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

namespace Lunch.WebApi.Controllers
{
    public class MenuController : ApiController
    {
        //GET api/menu
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

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
                        DishPicture = new DishPictureModel { Id = menuDetails.Dish.DishPicture.Id, Thumbnail = menuDetails.Dish.DishPicture.Thumbnail },
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

        // GET api/menu/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/menu
        public void Post([FromBody]string value)
        {
        }

        // PUT api/menu/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/menu/5
        public void Delete(int id)
        {
        }
    }
}
