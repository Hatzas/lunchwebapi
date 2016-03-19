using Lunch.DataAccessLayer.Repositories;
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
        public IEnumerable<object> Get()
        {
            var loggingUnitOfWork = new LunchUnitOfWork();
            var dishes = loggingUnitOfWork.DishRepository.GetAllDishes();

            return dishes.Select(d => new
            {
                Id = d.Id,
                Name = d.Name,
                Type = d.Type,
                Thumbnail = d.DishPicture != null ? d.DishPicture.Thumbnail : null
            });
        }

        // GET: api/Dish/5
        public object Get(int id)
        {
            var loggingUnitOfWork = new LunchUnitOfWork();
            var dish = loggingUnitOfWork.DishRepository.Find(id);

            return new
            {
                Id = dish.Id,
                Name = dish.Name,
                Type = dish.Type,
                Thumbnail = dish.DishPicture != null ? dish.DishPicture.Thumbnail : null
            };
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
