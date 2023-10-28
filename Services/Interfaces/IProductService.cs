using Data.Dtos.Requests;
using Data.Dtos.Responses;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> AddProduct(Guid companyId, CreateProductDto createProduct);

        Task<IEnumerable<ProductDto>> GetAllProducts(Guid companyId);

        Task<IEnumerable<ProductDto>> GetAllProducts(string companyName);

        Task<ProductDto> GetProductById(Guid productId);

        Task<ProductDto> EditProduct(Guid productId, EditProductDto model);
    }
}
