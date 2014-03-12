using System;
using System.Collections.Generic;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using System.IO;

using Rhino.Mocks;

namespace twg.chk.DataService.Tests.FrontOffice.Acceptance
{
    public class FakeOwinContext : IOwinContext
    {
        public FakeOwinContext()
        {
            Response = MockRepository.GenerateStub<IOwinResponse>();
        }

        public Microsoft.Owin.Security.IAuthenticationManager Authentication { get; set; }
        public IDictionary<string, object> Environment { get; set; }
        public IOwinRequest Request { get; set; }
        public IOwinResponse Response { get; set; }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }
        public IOwinContext Set<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public TextWriter TraceOutput
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
