using Data.Dtos.Requests;

namespace Services.Interfaces
{
    public interface IProductService
    {
        Task<bool> AddProduct(Guid companyId, CreateProductDto createProduct);
    }
}
