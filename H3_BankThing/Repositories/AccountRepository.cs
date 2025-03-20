using H3_BankThing.Data;
using H3_BankThing.Interfaces;
using H3_BankThing.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Repositories
{
    /// <summary>
    /// Provides repository methods for handling bank account data within the application database.
    /// Implements the <see cref="IAccountRepository"/> interface.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRepository"/> class.
        /// </summary>
        /// <param name="context">The application's database context.</param>
        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves an account from the database based on the provided account number and PIN code.
        /// </summary>
        /// <param name="accountNumber">The unique identifier for the bank account.</param>
        /// <param name="pinCode">The PIN code associated with the account.</param>
        /// <returns>
        /// A <see cref="BankAccount"/> object if a matching account is found; otherwise, <c>null</c>.
        /// </returns>
        public BankAccount? GetAccount(string accountNumber, string pinCode)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber && a.PinCode == pinCode);

        }

        /// <summary>
        /// Adds a new bank account to the database if it does not already exist.
        /// </summary>
        /// <param name="account">The bank account to be added.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an account with the same account number already exists.
        /// </exception>
        public void AddAccount(BankAccount account)
        {
            if (DoesAccountExist(account))
            {
                throw new InvalidOperationException("An account with this account number already exists.");
            }

            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates an existing bank account in the database.
        /// </summary>
        /// <param name="account">The updated bank account object.</param>
        public void UpdateAccount(BankAccount account)
        {
            _context.Accounts.Update(account);
            _context.SaveChanges();
        }

        /// <summary>
        /// Checks whether an account with the same account number already exists in the database.
        /// </summary>
        /// <param name="account">The bank account to check for existence.</param>
        /// <returns><c>true</c> if the account exists; otherwise, <c>false</c>.</returns>
        public bool DoesAccountExist(BankAccount account)
        {
            BankAccount? existingAccount = _context.Accounts
                .FirstOrDefault(a => a.AccountNumber == account.AccountNumber);

            if (existingAccount == null)
            {
                return false;
            }

            return true;
        }
    }
}
