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

        public static int Role()
        {
            _custom = new CustomIdentity();
            if (!_custom.IsAuthenticated)
            {
                return 1;
            }

            return _custom.Role;
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

        public static string Channel()
        {
            _custom = new CustomIdentity();
            return _custom.Channel;
        }

        public static bool isAuthenticated()
        {
            _custom = new CustomIdentity();
            return _custom.IsAuthenticated;
        }
    }
}
