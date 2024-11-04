namespace Adspro.Contract.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
}
