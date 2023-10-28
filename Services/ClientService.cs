using Data;
using Data.Dtos.Requests;
using Data.Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Services
{
    public class ClientService : IClientService
    {
        private readonly DbContext _context;
        private readonly HttpClient _client;
        private const string apiUrl= "https://api.coingecko.com/api/v3/simple/price?ids=ethereum&vs_currencies=usd";
        public ClientService(DbContext context,HttpClient client)
        {
                _context = context;
                _client =  client;
        }

        public async Task<ClientDto> Login(LoginClientDto client)
        {
            var clientCompany = await _context.Set<Client>().Where(c => c.Email == client.Email).FirstOrDefaultAsync();

            if (clientCompany == null)
            {
                throw new ArgumentException("Company not found");
            }

            if (!VerifyPassword(client.Password, clientCompany.PasswordHash, clientCompany.PasswordSalt))
            {
                throw new ArgumentException("Wrong password");
            }

            return new ClientDto
            {
                Id = clientCompany.Id,
                FirsName = clientCompany.FirstName,
                LastName= clientCompany.LastName,
                Email = clientCompany.Email,
                Role = clientCompany.Role,
            };
        }

        public async Task<CriptoPaymentResponse> Pay(string clientPrivateKey,string companyAccount,string clientAccount,decimal amount)
        {
            var ethAmount =await CheckConversionRate(amount);
            string testnetUrl = "https://proud-orbital-gas.ethereum-sepolia.quiknode.pro/651ff2ef76bf55dfcabb8dd417d964cf65790261/";
            var account = new Account(clientPrivateKey);
            var web3 = new Web3(account, testnetUrl);
            var toAddress = companyAccount;
            var balanceWei = await web3.Eth.GetBalance.SendRequestAsync(clientAccount);
            var balanceEther = Web3.Convert.FromWei(balanceWei);
            if (balanceEther<ethAmount)
            {
                return new CriptoPaymentResponse { Error="This clientAccount doesn't have enought money"};
            }
            var transactionReceipt = await web3.Eth.GetEtherTransferService()
            .TransferEtherAndWaitForReceiptAsync(toAddress, ethAmount, 2);
            Console.WriteLine($"Transaction {transactionReceipt.TransactionHash} for amount of {ethAmount} Ether completed");
            return new CriptoPaymentResponse {TransactionHash=transactionReceipt.TransactionHash,Amount=ethAmount };
        }
        private async Task<decimal> CheckConversionRate(decimal amount)
        {
            decimal ethAmount = 0;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    await Console.Out.WriteLineAsync(jsonResult);
                    // Parse the JSON to get the exchange rate
                    var res = jsonResult.Split(':')[2].TrimEnd('}');
                    decimal ethereumToUsdRate = Convert.ToDecimal(res);


                    // Perform the conversion
                    decimal usdAmount = amount; // Replace with the amount in USD you want to convert
                     ethAmount = usdAmount / ethereumToUsdRate;

                }
                else
                {
                    Console.WriteLine("Error: " + response.ReasonPhrase);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("HTTP Request Error: " + e.Message);
            }
            return ethAmount;
        }


        public async Task<ClientDto> Register(RegisterClientDto client)
        {
            Client clientEntity = new Client()
            {
                FirstName=client.FirstName,
                LastName=client.LastName,
                Email=client.Email,
                

            };
            CreatePasswordHash(client.Password, out byte[] passwordHash, out byte[] passwordSalt);
            clientEntity.PasswordSalt = passwordSalt;
            clientEntity.PasswordHash = passwordHash;
            await _context.AddAsync<Client>(clientEntity);
            await _context.SaveChangesAsync();
            var createdClient = await _context.Set<Client>().Where(client => client.Email == clientEntity.Email).FirstOrDefaultAsync();
            return new ClientDto
            {
                Id = createdClient.Id,
                FirsName =client.FirstName,
                LastName=client.LastName, 
                Email=client.Email,
                Role=createdClient.Role,
            };

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
