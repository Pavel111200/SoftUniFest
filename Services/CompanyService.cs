using Data;
using Data.Dtos.Requests;
using Data.Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System.Security.Cryptography;

namespace Services
{
    public class CompanyService : ICompanyService
    {
        private readonly DbContext _context;
        public CompanyService(DbContext context)
        {
            _context = context;
        }
        public async Task<CompanyDto> Register(RegisterCompanyDto company)
        {
            Company companyEntity = new Company()
            {
                Name = company.Name,
                Email = company.Email
            };
            
            CreatePasswordHash(company.Password, out byte[] passwordHash, out byte[] passwordSalt);
            companyEntity.PasswordSalt = passwordSalt;
            companyEntity.PasswordHash = passwordHash;

            await _context.AddAsync<Company>(companyEntity);
            await _context.SaveChangesAsync();

            var createdCompany = await _context.Set<Company>().Where(company => company.Email == companyEntity.Email).FirstOrDefaultAsync();

            return new CompanyDto()
            {
                Name = createdCompany.Name,
                Email = createdCompany.Email,
                Id = createdCompany.Id,
                Role= createdCompany.Role,
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
    }
}
