using Lunch.DataAccessLayer.Repositories;
using Lunch.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lunch.WebApi.Controllers
{
    public class DishController : ApiController
    {
        // GET: api/Dish
        public HttpResponseMessage Get()
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();
                var dishes = loggingUnitOfWork.DishRepository.GetAllDishes();

                return Request.CreateResponse(dishes.Select(d => new
                {
                    Id = d.Id,
                    Name = d.Name,
                    Type = d.Type,
                    Thumbnail = d.DishPicture != null ? d.DishPicture.Thumbnail : null
                }));
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dish Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // GET: api/Dish/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();
                var dish = loggingUnitOfWork.DishRepository.Find(id);

                return Request.CreateResponse(new
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Type = dish.Type,
                    Thumbnail = dish.DishPicture != null ? dish.DishPicture.Thumbnail : null
                });
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dish/{id} Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // POST: api/Dish
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Dish/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Dish/5
        public void Delete(int id)
        {
        }
    }
}
