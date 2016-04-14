using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Lunch.DataAccessLayer.Repositories;
using Lunch.Logging;
using Lunch.Model;
using Lunch.WebApi.Helpers;
using Lunch.WebApi.Helpers.CustomException;
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

                return Request.CreateResponse(new
                {
                    Id = dish.Id,
                    Message = HttpStatusCode.OK,
                });
                //return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dish Post: ", ex);
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        }

        public Image StringToImage(string inputString)
        {
            byte[] imageBytes = Encoding.Unicode.GetBytes(inputString);

            // Don't need to use the constructor that takes the starting offset and length
            // as we're using the whole byte array.
            MemoryStream ms = new MemoryStream(imageBytes);

            Image image = Image.FromStream(ms, true, true);

            return image;
        }

        //[HttpPost]
        //[Route("api/dish/thumbnail")]
        //public HttpResponseMessage PostDishThumbnail(DishesModel model)
        //{
        //    try
        //    {
        //        //Convert image to thumbnail
        //        string base64String;

        //        var picture = StringToImage(model.DishPicture.Thumbnail);
        //        Image thumbnail = picture.GetThumbnailImage(320, 230, () => false, IntPtr.Zero);
        //        using (Image image = thumbnail)
        //        {
        //            using (MemoryStream m = new MemoryStream())
        //            {
        //                image.Save(m, image.RawFormat);
        //                byte[] imageBytes = m.ToArray();

        //                // Convert byte[] to Base64 String
        //                base64String = Convert.ToBase64String(imageBytes);
        //            }
        //        }

        //        //-------------

        //        var lunchUnitOfWork = new LunchUnitOfWork();
        //        var dish = lunchUnitOfWork.DishRepository.GetDishById(model.Id);
        //        if (dish != null)
        //        {
        //            var dishPicture = new DishPicture();
        //            dishPicture.Id = model.DishPicture.Id;
        //            dishPicture.Thumbnail = StringToByteArray(base64String);

        //            lunchUnitOfWork.DishPictureRepository.Upsert(dishPicture);
        //            lunchUnitOfWork.Save();


        //            dish.DishPictureId = model.DishPicture.Id;
        //            lunchUnitOfWork.Save();
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.For(this).Error("api/dish/thumbnail Post: ", ex);
        //    }

        //    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
        //}


        [HttpPost]
        [Route("api/dish/thumbnail")]
        public async Task<HttpResponseMessage> PostDishThumbnail()
        {
            var tempFolder = ControllerHelper.GetNewTemporaryFolder();
            MultipartFormDataStreamProvider provider = null;
            try
            {
                // Check if the request contains multipart/form-data.
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden,
                        "This request is not properly formatted");
                }
                if (!Directory.Exists(tempFolder.FullFolderPath))
                    Directory.CreateDirectory(tempFolder.FullFolderPath);

                provider = new MultipartFormDataStreamProvider(tempFolder.FullFolderPath);

                // Read the form data and return an async task.
                await Request.Content.ReadAsMultipartAsync(provider);


                //get the form data.
                Dictionary<string, string> formData = ControllerHelper.GetFormDataDictionaryFromJson(provider.FormData);

                //validate configuration form data
                FormData thumbnailFormData = ControllerHelper.ValidateFormData(formData);


                //Validate resource file upload
                var fileUploadError = string.Empty;
                MultipartFileData file = ControllerHelper.ValidateFileUpload(provider.FileData);

                var fileInfo = new FileInfo(file.LocalFileName);
                var fileName = ControllerHelper.GetFileNameFromHeader(file.Headers.ContentDisposition);

                //---------------


                //Convert image to thumbnail
                //string base64String;
                
                //var picture = StringToImage(model.DishPicture.Thumbnail);
                var picture = Image.FromFile(fileInfo.FullName);
               // var picture2 = Image.FromStream(file);

                byte[] thumbNew;
                //---------------------------------------
                using (var srcImage = Image.FromFile(fileInfo.FullName))
                using (var newImage = new Bitmap(100, 100))
                using (var graphics = Graphics.FromImage(newImage))
                using (var stream = new MemoryStream())
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(srcImage, new Rectangle(0, 0, 320, 230));
                    newImage.Save(stream, ImageFormat.Png);
                    
                   // var thumbNew = File(stream.ToArray(), "image/png");
                    thumbNew = stream.ToArray();

                    //artwork.ArtworkThumbnail = thumbNew.FileContents;
                }    

                //---------------------------------------
                
                //Image thumbnail = picture.GetThumbnailImage(320, 230, () => false, IntPtr.Zero);
                //using (Image image = thumbnail)
                //{
                //    using (MemoryStream m = new MemoryStream())
                //    {
                //        image.Save(m, ImageFormat.Bmp);
                //        byte[] imageBytes = m.ToArray();

                //        // Convert byte[] to Base64 String
                //        base64String = Convert.ToBase64String(imageBytes);
                //    }
                //}

                //-------------

                var lunchUnitOfWork = new LunchUnitOfWork();
                int dishIdValue;
                if (Int32.TryParse(thumbnailFormData.DishId, out dishIdValue))
                {
                    var dish = lunchUnitOfWork.DishRepository.GetDishById(dishIdValue);
                    if (dish != null)
                    {
                        var dishPicture = new DishPicture();
                        //dishPicture.Id = model.DishPicture.Id;
                        //dishPicture.Thumbnail = StringToByteArray(base64String);
                        dishPicture.Thumbnail = thumbNew;

                        lunchUnitOfWork.DishPictureRepository.Upsert(dishPicture);
                        lunchUnitOfWork.Save();


                        dish.DishPictureId = dishPicture.Id;
                        lunchUnitOfWork.Save();
                    }

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (CustomValidationException ex)
            {

                //cleanup temp folder
                if (tempFolder != null)
                    ControllerHelper.DirectoryCleanup(new List<string> { tempFolder.FullFolderPath });

                return Request.CreateErrorResponse(ex.HttpResponseStatus, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dish/thumbnail Post: ", ex);
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

        [HttpPost]
        [Route("api/dish/rating")]
        public HttpResponseMessage DishRating(DishRatingModel model)
        {
            try
            {
                var lunchUnitOfWork = new LunchUnitOfWork();

                var user = lunchUnitOfWork.UserRepository.GetUserByName(model.UserId);
                var dish = lunchUnitOfWork.DishRepository.Find(model.DishId);
                var userDishStats = lunchUnitOfWork.DishStatisticsRepository.GetDishStatistic(model.DishId, user.Id);


                if (userDishStats == null)
                {
                    userDishStats = new DishStats
                    {
                        Rating = model.Rating,
                        UserId = user.Id,
                        DishId = dish.Id
                    };
                }
                else
                {
                    userDishStats.Rating = model.Rating;
                }


                lunchUnitOfWork.DishStatisticsRepository.Upsert(userDishStats);
                lunchUnitOfWork.Save();


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Logger.For(this).Error("api/dish/rating Post: ", ex);
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

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
