using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Security;
using System.Web;

namespace Evolantis.Authentication
{
    public class AuthenticatedUser
    {
        private static CustomIdentity _custom;

        public static int ID()
        {
            _custom = new CustomIdentity();
            return _custom.ID;
        }
           
        public static string Email()
        {
            _custom = new CustomIdentity();
            return _custom.Email;
        }
        
        public static string Username()
        {
            _custom = new CustomIdentity();
            return _custom.Username;
        }

        public static string Type()
        {
            _custom = new CustomIdentity();
            return _custom.Type;
        }

        public static string Firstname()
        {
            _custom = new CustomIdentity();
            return _custom.FirstName;
        }

        public static string Lastname()
        {
            _custom = new CustomIdentity();
            return _custom.LastName;
        }

        public static string IP()
        {
            _custom = new CustomIdentity();
            return _custom.IP;
        }

        public static string UserAgent()
        {
            _custom = new CustomIdentity();
            return _custom.UserAgent;
        }

        public static string ExtendedObject()
        {
            _custom = new CustomIdentity();
            return _custom.ExtendedObject;
        }

        public static bool IsAuthenticated()
        {
            _custom = new CustomIdentity();
            return _custom.IsAuthenticated;
        }

        public static T Role<T>()
        {
            _custom = new CustomIdentity();
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertTo((object)_custom.Role, typeof(T));
        }

        public static bool Role<T>(T value)
        {
            _custom = new CustomIdentity();

            if (HttpContext.Current.User.IsInRole(TypeDescriptor.GetConverter(typeof(T)).ConvertTo((object)value, typeof(T)).ToString()))
                return true;
            return false;
        }

        public static T ExtendedObject<T>()
        {
            _custom = new CustomIdentity();
            return JsonConvert.DeserializeObject<T>(_custom.ExtendedObject);
        }

    }
}
