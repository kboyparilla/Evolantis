using System;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web;

namespace Evolantis.Authentication
{
    public class CustomIdentity
    {
        private FormsAuthenticationTicket ticket = null;
        private UserIdentity user;

        public CustomIdentity()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                ticket = FormsAuthentication.Decrypt(authCookie.Value);
                //deserialize the userdata back into user identity
                user = JsonConvert.DeserializeObject<UserIdentity>(ticket.UserData);
            }
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return ticket != null; }
        }

        public string Email
        {
            get { return user.Email; }
        }

        public string Username
        {
            get { return user.Username; }
        }

        public string FirstName
        {
            get { return user.FirstName; }
        }

        public string LastName
        {
            get { return user.LastName; }
        }

        public int Role
        {
            get { return user.Role; }
        }

        public string Type
        {
            get { return user.Type; }
        }

        public int ID
        {
            get { return user.ID; }
        }

        public string IP
        {
            get { return user.IP; }
        }

        public string UserAgent
        {
            get { return user.UserAgent; }
        }

        public string Channel
        {
            get { return user.Channel; }
        }
    }
}
