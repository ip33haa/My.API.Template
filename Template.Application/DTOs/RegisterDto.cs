namespace Template.Application.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
    }
}
