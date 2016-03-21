using Lunch.DataAccessLayer.Repositories;
using Lunch.Logging;
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
        public HttpResponseMessage Get()
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();
                var dishCategories = loggingUnitOfWork.DishCategoryRepository.GetAllDishCategories();

                return Request.CreateResponse(dishCategories.Select(dc => new { Id = dc.Id, Name = dc.Name }));
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dishcategory Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // GET: api/DishCategory/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();
                var dishCategory = loggingUnitOfWork.DishCategoryRepository.Find(id);

                return Request.CreateResponse(new { Id = dishCategory.Id, Name = dishCategory.Name });
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dishcategory/{id} Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // POST: api/DishCategory
        public HttpResponseMessage Post([FromBody]string name)
        {
            try
            {
                var dishCategory = new DishCategory
                {
                    Name = name,
                };

                var loggingUnitOfWork = new LunchUnitOfWork();
                loggingUnitOfWork.DishCategoryRepository.Upsert(dishCategory);
                loggingUnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dishcategory Post: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // PUT: api/DishCategory/5
        public HttpResponseMessage Put(int id, [FromBody]string name)
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();
                var dishCategory = loggingUnitOfWork.DishCategoryRepository.Find(id);

                dishCategory.Name = name;

                loggingUnitOfWork.DishCategoryRepository.Upsert(dishCategory);
                loggingUnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dishcategory/{id} Put: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // DELETE: api/DishCategory/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();
                var dishCategory = loggingUnitOfWork.DishCategoryRepository.Find(id);

                loggingUnitOfWork.DishCategoryRepository.DeleteEntity(dishCategory);
                loggingUnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dishcategory/{id} Delete: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}
