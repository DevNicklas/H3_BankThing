using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Models
{
    public class BankAccount
    {
        public required string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0;
        public required string PinCode { get; set; } = string.Empty;
    }
}
