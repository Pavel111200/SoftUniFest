using Data.Dtos.Requests;
using Data.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SoftUniFest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IClientService _clientService;
        public ClientsController(IConfiguration configuration, IClientService clientService)
        {
            _configuration = configuration;
            _clientService = clientService;

        }
        [HttpPost("register")]
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(RegisterClientDto client)
        {
            ClientDto result = new ClientDto();
            try
            {
                result = await _clientService.Register(client);
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
        public async Task<IActionResult> Login(LoginClientDto client)
        {
            ClientDto result = new ClientDto();
            try
            {
                result = await _clientService.Login(client);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            result.AccessToken = CreateToken(result);

            return Ok(result);
        }
        [HttpPost("payWithCripto")]
        [ProducesResponseType(typeof(CriptoPaymentResponse) ,StatusCodes.Status200OK)]
        public async Task<IActionResult> PayWithCripto(string clientPrivateKey,decimal amount,string companyAccount)
        {
            var result = await _clientService.Pay(clientPrivateKey, companyAccount, amount);
           
            return Ok(result);
        }
        private string CreateToken(ClientDto client)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, client.Role)
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
