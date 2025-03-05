namespace Template.Domain.Entities
{
    public class SIPOC : BaseEntity
    {
        public Guid SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!; // Navigation Property

        public string Input { get; set; } = string.Empty;
        public string Challenges1 { get; set; } = string.Empty;
        public string Process { get; set; } = string.Empty;
        public string Challenges2 { get; set; } = string.Empty;
        public string Outputs { get; set; } = string.Empty;

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null!; // Navigation Property
    }
}
