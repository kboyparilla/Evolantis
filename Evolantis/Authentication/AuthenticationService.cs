using System;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web;

namespace Evolantis.Authentication
{
    public class AuthenticationService
    {
        public static void Signin(UserIdentity user, bool createPersistentCookie)
        {
            //UserData is stored as json
            string userData = JsonConvert.SerializeObject(user);

            FormsAuthenticationTicket authTicket = new
                FormsAuthenticationTicket(1, //version
                                          user.ID.ToString(), // user name
                                          DateTime.Now,             //creation
                                          DateTime.Now.AddMinutes(120), //Expiration
                                          createPersistentCookie, userData); //storing the json data

            string encTicket = FormsAuthentication.Encrypt(authTicket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket)
            {
                Expires = authTicket.Expiration,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void Signout()
        {
            FormsAuthentication.SignOut();
        }
    }
}
