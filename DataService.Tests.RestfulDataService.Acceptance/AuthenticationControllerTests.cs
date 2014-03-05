using System;
using System.Net.Http;
using NUnit.Framework;
using Rhino.Mocks;
using Ninject;

using twg.chk.DataService.FrontOffice;

namespace twg.chk.DataService.Tests.FrontOffice.Acceptance
{
    /// <summary>
    /// ---------------------------
    /// Token controller Test class
    /// ---------------------------
    /// Contains a single method to request a token for an existing account
    /// Request need to contain the account user name and password
    /// If user name and password are valid we return the corresponding token
    /// else a bad request message is returned
    /// </summary>
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private IKernel _kernel;
        private AuthenticationController _objectUnderTest;
        private IAuthenticationManager _fakeAuthenticationManager;

        [SetUp]
        public void Setup()
        {
            _kernel = new StandardKernel();
            Startup.NinjectConfig.CreateKernel(_kernel);

            _fakeAuthenticationManager = MockRepository.GenerateMock<IAuthenticationManager>();
            _objectUnderTest = new AuthenticationController(_fakeAuthenticationManager);
        }

        #region Token

        [Test]
        public void Token_RequestToken()
        {
            // Existing Account authentication
            var httpMessage = _objectUnderTest.Token("ravi", "Passw0rd");
            Assert.IsTrue(httpMessage.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.NotNull(httpMessage.Content);

            // Non-Existing Account authentication
            var httpMessage2 = _objectUnderTest.Token("dummy", "dummy");
            Assert.IsTrue(httpMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized);
            Assert.Null(httpMessage.Content);
        }

        #endregion
    }
}
