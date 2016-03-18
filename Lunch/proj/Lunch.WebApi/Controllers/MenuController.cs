using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunch.DataAccessLayer.Repositories;
using Lunch.Model.Extended;

namespace Lunch.WebApi.Controllers
{
    public class MenuController : ApiController
    {
        // GET api/menu
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public IEnumerable<MenuDetails> Get(DateTime startDate, DateTime endDate)
        {
           var loggingUnitOfWork = new LunchUnitOfWork();
           var menuList = loggingUnitOfWork.MenuRepository.GetMenusDetailsByStartDateAndEndDate(startDate, endDate);
           return menuList;
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
