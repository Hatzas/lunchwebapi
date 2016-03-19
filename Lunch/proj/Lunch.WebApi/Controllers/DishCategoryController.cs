using Lunch.DataAccessLayer.Repositories;
using Lunch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lunch.WebApi.Controllers
{
    public class DishCategoryController : ApiController
    {
        // GET: api/DishCategory
        public IEnumerable<object> Get()
        {
            var loggingUnitOfWork = new LunchUnitOfWork();
            var dishCategories = loggingUnitOfWork.DishCategoryRepository.GetAllDishCategories();

            return dishCategories.Select(dc => new { Id = dc.Id, Name = dc.Name });
        }

        // GET: api/DishCategory/5
        public object Get(int id)
        {
            var loggingUnitOfWork = new LunchUnitOfWork();
            var dishCategory = loggingUnitOfWork.DishCategoryRepository.Find(id);

            return new { Id = dishCategory.Id, Name = dishCategory.Name };
        }

        // POST: api/DishCategory
        public void Post([FromBody]string name)
        {
            var dishCategory = new DishCategory
            {
                Name = name,
            };

            var loggingUnitOfWork = new LunchUnitOfWork();
            loggingUnitOfWork.DishCategoryRepository.Upsert(dishCategory);
            loggingUnitOfWork.Save();
        }

        // PUT: api/DishCategory/5
        public void Put(int id, [FromBody]string name)
        {
            var loggingUnitOfWork = new LunchUnitOfWork();
            var dishCategory = loggingUnitOfWork.DishCategoryRepository.Find(id);

            dishCategory.Name = name;

            loggingUnitOfWork.DishCategoryRepository.Upsert(dishCategory);
            loggingUnitOfWork.Save();
        }

        // DELETE: api/DishCategory/5
        public void Delete(int id)
        {
            var loggingUnitOfWork = new LunchUnitOfWork();
            var dishCategory = loggingUnitOfWork.DishCategoryRepository.Find(id);

            loggingUnitOfWork.DishCategoryRepository.DeleteEntity(dishCategory);
            loggingUnitOfWork.Save();
        }
    }
}
