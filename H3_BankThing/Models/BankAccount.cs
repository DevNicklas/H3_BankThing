using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Models
{
    /// <summary>
    /// Represents a bank account with account number, balance, and PIN code.
    /// </summary>
    public class BankAccount
    {
        [Key]
        public required string AccountNumber { get; set; } = string.Empty;

        public decimal Balance { get; set; } = 0;

        public required string PinCode { get; set; } = string.Empty;
    }
}
