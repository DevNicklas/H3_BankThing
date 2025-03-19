using H3_BankThing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Interfaces
{
    /// <summary>
    /// Defines the contract for account repository operations.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Retrieves a bank account based on the provided account number and PIN code.
        /// </summary>
        /// <param name="accountNumber">The unique identifier of the bank account.</param>
        /// <param name="pinCode">The PIN code associated with the account for authentication.</param>
        /// <returns>A <see cref="BankAccount"/> object if found; otherwise, null.</returns>
        BankAccount GetAccount(string accountNumber, string pinCode);

        /// <summary>
        /// Add the account in the system
        /// </summary>
        /// <param name="account">The bank account object to be added.</param>
        void AddAccount(BankAccount account);
    }
}
