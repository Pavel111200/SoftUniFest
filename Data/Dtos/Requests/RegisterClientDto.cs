using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos.Requests
{
    public class RegisterClientDto
    {
        
        public string FirsName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } =string.Empty;
        public string Password { get; set; } = string.Empty;
        
    }
}
