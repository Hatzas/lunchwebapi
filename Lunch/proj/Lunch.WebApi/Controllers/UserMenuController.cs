using Lunch.DataAccessLayer.Repositories;
using Lunch.Logging;
using Lunch.Model.Extended;
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
        // GET: api/UserMenu
        public HttpResponseMessage Get(DateTime startDate, DateTime endDate)
        {
            try
            {
                var loggingUnitOfWork = new LunchUnitOfWork();
                var userMenus = loggingUnitOfWork.UserMenuRepository.GetUserMenusDetailsByInterval(startDate, endDate);

                return Request.CreateResponse(userMenus);
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
    }
}
