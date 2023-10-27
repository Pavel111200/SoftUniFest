using Data.Dtos.Requests;
using Data.Dtos.Responses;

namespace Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDto> Register(RegisterCompanyDto company);
    }
}
