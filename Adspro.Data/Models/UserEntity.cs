namespace Adspro.Data.Models
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool Active { get; set; }
    }
}
