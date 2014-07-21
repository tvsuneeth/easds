using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;

using TWG.EASDataService.Business;
using TWG.EASDataService.Services;

namespace TWG.EASDataService.BackOffice.Controllers
{
    [RoutePrefix("accounts")]
    public class AccountController : Controller
    {
        private UserService _userService;
        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Paged list of user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{page:int?}", Name = "GetAccountIndex")]
        public ActionResult Index(int page = 1)
        {
            var users = _userService.GetPaged(page, 20);
            return View(users);
        }

        [HttpPost]
        public JsonResult Create()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public JsonResult BackOfficeAccess(int id,[System.Web.Http.FromBody]bool hasBackOfficeAccess)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public JsonResult FrontOfficeAccess(int id, [System.Web.Http.FromBody]bool hasFrontOfficeAccess)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public JsonResult ResetAccountPassword(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public JsonResult AccountActivation(int id, [System.Web.Http.FromBody]bool isActiveAccount)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public JsonResult DeleteAccount(int id)
        {
            throw new NotImplementedException();
        }
	}
}