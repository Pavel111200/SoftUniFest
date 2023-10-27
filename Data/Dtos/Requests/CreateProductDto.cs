using System.ComponentModel.DataAnnotations;

namespace Data.Dtos.Requests
{
    public class CreateProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
