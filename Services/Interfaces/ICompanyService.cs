using Data.Dtos.Requests;
using Data.Dtos.Responses;

namespace Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDto> Register(RegisterCompanyDto company);

        Task<CompanyDto> Login(LoginCompanyDto company);
        Task<List<CompanyDto>> GetAllVendors();

        Task<List<CompanyDto>>GetByStr(string str);
    }
}
