using Data;
using Data.Dtos.Requests;
using Data.Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly DbContext _context;
        public ProductService(DbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProduct(Guid companyId, CreateProductDto createProduct)
        {
            Product productToCreate = new Product
            {
                Name = createProduct.Name,
                Description = createProduct.Description,
                Price = createProduct.Price,
                CompanyId = companyId,
            };

            try
            {
                await _context.AddAsync(productToCreate);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts(Guid companyId)
        {
            return await _context.Set<Product>()
                .Where(p => p.CompanyId == companyId)
                .Select(p => new ProductDto { Id = p.Id, Name = p.Name, Description = p.Description, Price = p.Price })
                .ToListAsync();
        }

        public async Task<ProductDto> GetProductById(Guid productId)
        {
            Product? product = await _context.Set<Product>()
                .Where(p => p.Id == productId)
                .FirstOrDefaultAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }
    }
}
