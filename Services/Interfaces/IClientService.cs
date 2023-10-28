using Data.Dtos.Requests;
using Data.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IClientService
    {
        Task<ClientDto> Register(RegisterClientDto client);
        Task<ClientDto> Login(LoginClientDto company);
        Task<CriptoPaymentResponse> Pay(string clientPrivateKey, string companyAccount, decimal amount);
    }
}
