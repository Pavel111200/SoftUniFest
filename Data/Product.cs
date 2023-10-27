using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0,double.MaxValue, ErrorMessage = "Value cannot be negative.")]
        public decimal Price { get; set; }

        [ForeignKey(nameof(Company))]
        public Guid UserId { get; set; }

        public Company Company { get; set; }

        public IEnumerable<Client> Clients { get; set; } = new List<Client>();
    }
}
