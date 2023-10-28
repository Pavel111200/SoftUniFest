using Data.Dtos.Requests;
using Data.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SoftUniFest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService; 
        private readonly IConfiguration _configuration;
        public CompaniesController(ICompanyService companyService, IConfiguration configuration)
        {
            _companyService = companyService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(RegisterCompanyDto company)
        {
            CompanyDto result = new CompanyDto();
            try
            {
                result = await _companyService.Register(company);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            result.AccessToken = CreateToken(result);

            return Ok(result);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginCompanyDto company)
        {
            CompanyDto result = new CompanyDto();
            try
            {
                result = await _companyService.Login(company);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            result.AccessToken = CreateToken(result);

            return Ok(result);
        }
        [HttpGet("getAllVendors")]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetAllVendor()
        {
            var result = await _companyService.GetAllVendors();
            return Ok(result);
        }
        [HttpGet("getByStr")]
        [Authorize(Roles = "Client")]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByStr(string str)
        {
            var result=await _companyService.GetByStr(str);
            return Ok(result);

        private string CreateToken(CompanyDto company)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, company.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
