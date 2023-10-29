using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; } = "Client";

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
