using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Gets or sets the unique identifier for the bank account.
        /// </summary>
        public required string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current balance of the bank account.
        /// </summary>
        public decimal Balance { get; set; } = 0;

        /// <summary>
        /// Gets or sets the PIN code associated with the bank account for security purposes.
        /// </summary>
        public required string PinCode { get; set; } = string.Empty;
    }
}
