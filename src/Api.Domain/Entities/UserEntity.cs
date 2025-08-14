namespace Api.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
