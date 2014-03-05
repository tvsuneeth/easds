using System;
namespace twg.chk.DataService.FrontOffice
{
    public interface IAuthenticationManager
    {
        String RequestToken(String userName, String password);
    }

    public class AuthenticationManager : IAuthenticationManager
    {

        public string RequestToken(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}