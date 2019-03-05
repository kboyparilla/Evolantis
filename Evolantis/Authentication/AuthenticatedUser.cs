using System.Collections.Generic;
using Newtonsoft.Json;

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

        public static string Role()
        {
            _custom = new CustomIdentity();
            return _custom.Role;
        }

        public static bool Role(string value)
        {
            _custom = new CustomIdentity();
            if (_custom.Role == value)
                return true;
            return false;
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

        public static List<T> ExtendedListObject<T>()
        {
            _custom = new CustomIdentity();
            return JsonConvert.DeserializeObject<List<T>>(_custom.ExtendedObject);
        }

        public static T ExtendedObject<T>()
        {
            _custom = new CustomIdentity();
            return JsonConvert.DeserializeObject<T>(_custom.ExtendedObject);
        }

        public static string ExtendedObject()
        {
            _custom = new CustomIdentity();
            return _custom.ExtendedObject;
        }

        public static bool isAuthenticated()
        {
            _custom = new CustomIdentity();
            return _custom.IsAuthenticated;
        }
    }
}
