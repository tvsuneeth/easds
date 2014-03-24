using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Reflection;
using System.Data.Entity;
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

namespace twg.chk.DataService.Tests.FrontOffice.Acceptance
{
    public class SqlCeConfiguration : DbConfiguration
    {
        public SqlCeConfiguration()
        {
            SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlCeConnectionFactory(System.Data.Entity.SqlServerCompact.SqlCeProviderServices.ProviderInvariantName));

            SetProviderServices(System.Data.Entity.SqlServerCompact.SqlCeProviderServices.ProviderInvariantName,
                System.Data.Entity.SqlServerCompact.SqlCeProviderServices.Instance);
        }
    }

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
        private OAuthProvider _objectUnderTest;
        private UserManager<IdentityUser> _userManager;

        #region Setup and Cleanup

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

        }

        [TestInitialize]
        public void Setup()
        {
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new twg.chk.DataService.DbContext.DataServiceEntities("Datasource=myTestDatabase.mdf")));

            _objectUnderTest = new OAuthProvider(_userManager);
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (var userContext = new twg.chk.DataService.DbContext.DataServiceEntities("Datasource=myTestDatabase.mdf"))
            {
                userContext.Database.Delete();
            }
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
