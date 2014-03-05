using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace twg.chk.DataService.FrontOffice
{
    public class AuthenticationController : ApiController
    {
        private IAuthenticationManager _authenticationManager;
        public AuthenticationController(IAuthenticationManager authenticationManager)
        {
            _authenticationManager = authenticationManager;
        }

        public HttpResponseMessage Token(String userName, String password)
        {
            throw new NotImplementedException();
        }
    }
}
