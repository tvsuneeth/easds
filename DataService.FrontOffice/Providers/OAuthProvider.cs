using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

using twg.chk.DataService.api;

namespace twg.chk.DataService.FrontOffice.Providers
{
    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly String _publicClientId;
        private readonly IAuthenticationService _authenticationService;

        public OAuthProvider(IAuthenticationService authenticationService)
        {
            if (authenticationService == null)
            {
                throw new ArgumentNullException("authenticationService");
            }

            _publicClientId = "self";
            _authenticationService = authenticationService;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var oAuthIdentity = await _authenticationService.RequestAuthenticationToken(context.UserName, context.Password);
                AuthenticationProperties properties = CreateProperties(context.UserName);
                AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                context.Validated(ticket);
            }
            catch (AuthenticationException ex)
            {
                context.SetError("invalid_grant", ex.Message);
                return;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string> { { "userName", userName } };
            return new AuthenticationProperties(data);
        }
    }
}