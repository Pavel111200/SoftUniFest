using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos.Responses
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string FirsName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set;} = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
