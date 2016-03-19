using Lunch.DataAccessLayer.Repositories;
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
        public IEnumerable<MenuDetails> Get(DateTime startDate, DateTime endDate)
        {
            var loggingUnitOfWork = new LunchUnitOfWork();
            var userMenus = loggingUnitOfWork.UserMenuRepository.GetUserMenusDetailsByInterval(startDate, endDate);

            return userMenus;
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
