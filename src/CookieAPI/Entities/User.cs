namespace CookieAPI.Entities
{
    public class User
    {
        public Guid Guid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
         public char Gender { get; set; }

    }
}
