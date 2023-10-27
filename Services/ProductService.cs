using Data;
using Data.Dtos.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    }
}
