using H3_BankThing.Interfaces;
using H3_BankThing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Repositories
{
    /// <summary>
    /// Provides implementation for account repository operations, such as retrieving and adding bank accounts.
    /// Implements the <see cref="IAccountRepository"/> interface.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        // In-memory list to store bank accounts
        private readonly List<BankAccount> _accounts = new List<BankAccount>();

        /// <summary>
        /// Retrieves a bank account based on the specified account number and PIN code.
        /// </summary>
        /// <param name="accountNumber">The unique identifier for the bank account.</param>
        /// <param name="pinCode">The PIN code associated with the account for security purposes.</param>
        /// <returns>A <see cref="BankAccount"/> object if found, or null if no matching account is found.</returns>
        public BankAccount GetAccount(string accountNumber, string pinCode)
        {
            return _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber && a.PinCode == pinCode);
        }

        /// <summary>
        /// Adds a new bank account to the repository.
        /// </summary>
        /// <param name="account">The bank account to be added to the repository.</param>
        public void AddAccount(BankAccount account)
        {
            _accounts.Add(account);
        }
    }
}
