using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twg.chk.DataService.api
{
    public class AuthenticationException: Exception
    {
        public AuthenticationException(String message) : base(message) { }
    }
}
