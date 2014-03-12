using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using twg.chk.DataService.api;

namespace twg.chk.DataService.FrontOffice
{
    public class AuthenticationController : ApiController
    {
        private IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public HttpResponseMessage Token(String userName, String password)
        {
            //HttpResponseMessage responseMessage;
            //var token = String.Empty;
            //if (_authenticationService.RequestAuthenticationToken(userName, password, out token))
            //{
            //    var tokenObject = new UserToken { Token = token };
            //    responseMessage = Request.CreateResponse<UserToken>(HttpStatusCode.OK, tokenObject);
            //}
            //else
            //{
            //    responseMessage = Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            //}

            //return responseMessage;
            throw new NotImplementedException();
        }

        public class UserToken
        {
            public String Token { get; set; }
        }
    }
}
