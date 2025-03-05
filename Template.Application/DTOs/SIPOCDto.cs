namespace Template.Application.DTOs
{
    public class SIPOCDto
    {
        public Guid SupplierId { get; set; }
        public string Input { get; set; } = string.Empty;
        public string Challenges1 { get; set; } = string.Empty;
        public string Process { get; set; } = string.Empty;
        public string Challenges2 { get; set; } = string.Empty;
        public string Outputs { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
    }
}
