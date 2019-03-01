namespace Evolantis.Authentication
{
    public class UserIdentity
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Role { get; set; }
        public string Type { get; set; }
        public string IP { get; set; }
        public string UserAgent { get; set; }
        public string Channel { get; set; }
    }
}
