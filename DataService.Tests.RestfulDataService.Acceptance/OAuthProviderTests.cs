using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Ninject;

using twg.chk.DataService.api;
using twg.chk.DataService.FrontOffice;
using twg.chk.DataService.FrontOffice.Providers;
using System.IO;
using System.Reflection;

namespace twg.chk.DataService.Tests.FrontOffice.Acceptance
{
    /// <summary>
    /// ---------------------------
    /// OAuthProvider Test class
    /// ---------------------------
    /// Contains mainly a method to request a token for an existing account
    /// Request need to contain the account user name and password
    /// If user name and password are valid we return the corresponding token
    /// else a bad request (Httpcode: 400) message is returned
    /// </summary>
    [TestClass]
    public class OAuthProviderTests
    {
        private IKernel _kernel;
        private OAuthProvider _objectUnderTest;
        private twg.chk.DataService.Tests.FakeDatabase.FakeDatabase _fakeDb;
        private UserManager<IdentityUser> _userManager;

        #region Setup and Cleanup
        [TestInitialize]
        public void Setup()
        {
            _fakeDb = new twg.chk.DataService.Tests.FakeDatabase.FakeDatabase("myTestDatabase.mdf");
            _fakeDb.Create();

            _kernel = new StandardKernel();
            Startup.NinjectConfig.CreateKernel(_kernel);
            _userManager = _kernel.Get<UserManager<IdentityUser>>();
            _objectUnderTest = new OAuthProvider(_userManager);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _fakeDb.Remove();
        }
        #endregion

        #region Token

        [TestMethod]
        public void Token_RequestToken()
        {
            //add an user
            var authService = _userManager;
            var identityResult = authService.Create<IdentityUser>(new IdentityUser { UserName = "ravi" }, "Passw0rd");

            // Non-Existing Account authentication
            var owinContext = new FakeOwinContext();
            var fakeContextNonExistingUser = new OAuthGrantResourceOwnerCredentialsContext(
                owinContext,
                new OAuthAuthorizationServerOptions(),
                null,
                "dummy",
                "dummy",
                new List<String>()
            );

            _objectUnderTest.GrantResourceOwnerCredentials(fakeContextNonExistingUser);

            Assert.IsNull(fakeContextNonExistingUser.Ticket);
            Assert.IsFalse(fakeContextNonExistingUser.IsValidated);

            // Existing Account authentication
            var owinContext2 = new FakeOwinContext();
            var fakeContextExistingUser = new OAuthGrantResourceOwnerCredentialsContext(
                owinContext2, 
                new OAuthAuthorizationServerOptions(), 
                null, 
                "ravi", 
                "Passw0rd", 
                new List<String>()
            );

            _objectUnderTest.GrantResourceOwnerCredentials(fakeContextExistingUser);

            Assert.IsNotNull(fakeContextExistingUser.Ticket);
            Assert.IsTrue(fakeContextExistingUser.IsValidated);
        }

        #endregion
    }
}
