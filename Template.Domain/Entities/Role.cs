namespace Template.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = string.Empty;

        // The Role can have multiple Users, this should be a collection
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
