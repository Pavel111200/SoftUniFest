using Data.Dtos.Requests;
using Data.Dtos.Responses;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> AddProduct(Guid companyId, CreateProductDto createProduct);

        Task<IEnumerable<ProductDto>> GetAllProducts(Guid companyId);
    }
}
