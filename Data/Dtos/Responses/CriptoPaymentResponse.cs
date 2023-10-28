using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos.Responses
{
    public class CriptoPaymentResponse
    {
        public string TransactionHash { get; set; }
        public decimal Amount { get; set; }
    }
}
