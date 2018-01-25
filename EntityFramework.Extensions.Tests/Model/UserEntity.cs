namespace EntityFramework.Extensions.Tests.Model
{
    public class UserEntity
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public UserTypeEnum Type { get; set; }
    }
}