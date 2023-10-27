using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; } = "Company";

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
