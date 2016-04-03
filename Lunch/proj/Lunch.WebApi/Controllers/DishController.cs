using Lunch.DataAccessLayer.Repositories;
using Lunch.Logging;
using Lunch.Model;
using Lunch.WebApi.Models;
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
        // GET: api/dish
        public HttpResponseMessage Get()
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();
                var dishes = lunchUnitOfWork.DishRepository.GetAllDishes();

                return Request.CreateResponse(dishes.Select(d => new
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
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

        // GET: api/dish/id
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();
                var dish = lunchUnitOfWork.DishRepository.Find(id);

                return Request.CreateResponse(new
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Description = dish.Description,
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

        // POST: api/dish
        public HttpResponseMessage Post(DishesModel model)
        {
            try
             {
                var lunchUnitOfWork = new LunchUnitOfWork();

                var dish = new Dish
                {
                    Name = model.Name,
                    Description = model.Description,
                    Type = model.Type,
                };

                lunchUnitOfWork.DishRepository.Upsert(dish);
                lunchUnitOfWork.Save();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dish Post: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // PUT: api/dish/{id}
        public HttpResponseMessage Put(int id, DishesModel model)
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();

                var dish = lunchUnitOfWork.DishRepository.Find(id);
                dish.Name = model.Name;
                dish.Description = model.Description;
                dish.Type = model.Type;

                lunchUnitOfWork.DishRepository.Upsert(dish);
                lunchUnitOfWork.Save();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dish Put: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        // DELETE: api/dish/{id}
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();

                var dish = lunchUnitOfWork.DishRepository.Find(id);

                lunchUnitOfWork.DishRepository.DeleteEntity(dish);
                lunchUnitOfWork.Save();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dish Put: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}
