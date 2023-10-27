using Azure.Core;
using Data;
using Data.Dtos.Requests;
using Data.Dtos.Responses;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Services
{
    public class CompanyService : ICompanyService
    {
        private readonly DbContext _context;
        public CompanyService(DbContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyDto>> GetAllVendors()
        {
            var companies= await _context.Set<Company>().ToListAsync();
            var result=new List<CompanyDto>();
            foreach (var company in companies)
            {
                var companyDto = new CompanyDto()
                {
                    Id= company.Id,
                    Name= company.Name,
                    Role=company.Role,
                    Email=company.Email,

                };
                result.Add(companyDto);
            }
            return result;
        }

        public async Task<List<CompanyDto>> GetByStr(string str)
        {
            var companies=await _context.Set<Company>().Where(x=>x.Name.Contains(str)).ToListAsync();
            var result = new List<CompanyDto>();
            foreach (var company in companies)
            {
                result.Add(new CompanyDto {Id=company.Id, Email=company.Email,Name=company.Name}) ;
            }
            return result;
        }

        public async Task<CompanyDto> Login(LoginCompanyDto company)
        {
            var foundCompany = await _context.Set<Company>().Where(c => c.Email == company.Email).FirstOrDefaultAsync();

            if (foundCompany == null)
            {
                throw new ArgumentException("Company not found");
            }

            if (!VerifyPassword(company.Password, foundCompany.PasswordHash, foundCompany.PasswordSalt))
            {
                throw new ArgumentException("Wrong password");
            }

            return new CompanyDto
            {
                Id = foundCompany.Id,
                Name = foundCompany.Name,
                Email = foundCompany.Email,
                Role = foundCompany.Role,
            };
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
