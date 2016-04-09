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
        // GET: api/dishcategory
        public HttpResponseMessage Get()
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();
                var dishCategories = lunchUnitOfWork.DishCategoryRepository.GetAllDishCategories();

                return Request.CreateResponse(dishCategories.Select(dc => new { Id = dc.Id, Name = dc.Name }));
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dishcategory Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // GET: api/dishcategory/{id}
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();
                var dishCategory = lunchUnitOfWork.DishCategoryRepository.Find(id);

                return Request.CreateResponse(new { Id = dishCategory.Id, Name = dishCategory.Name });
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dishcategory/{id} Get: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // POST: api/dishcategory
        public HttpResponseMessage Post([FromBody]string name)
        {
            try
            {
                var dishCategory = new DishCategory
                {
                    Name = name,
                };

                var lunchUnitOfWork = new LunchUnitOfWork();
                lunchUnitOfWork.DishCategoryRepository.Upsert(dishCategory);
                lunchUnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dishcategory Post: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // PUT: api/dishcategory/{id}
        public HttpResponseMessage Put(int id, [FromBody]string name)
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();
                var dishCategory = lunchUnitOfWork.DishCategoryRepository.Find(id);

                dishCategory.Name = name;

                lunchUnitOfWork.DishCategoryRepository.Upsert(dishCategory);
                lunchUnitOfWork.Save();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dishcategory/{id} Put: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // DELETE: api/dishcategory/{id}
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();
                var dishCategory = lunchUnitOfWork.DishCategoryRepository.Find(id);

                lunchUnitOfWork.DishCategoryRepository.DeleteEntity(dishCategory);
                lunchUnitOfWork.Save();

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
