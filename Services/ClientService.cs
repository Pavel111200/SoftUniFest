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

namespace Services
{
    public class ClientService:IClientService
    {
        private readonly DbContext _context;
        public ClientService(DbContext context)
        {
                _context = context;
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
