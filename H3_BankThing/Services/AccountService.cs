using H3_BankThing.Interfaces;
using H3_BankThing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Services
{
    /// <summary>
    /// Provides implementation for account-related services, such as creating accounts and withdrawing funds.
    /// Implements the <see cref="IAccountService"/> interface.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="accountRepository">The repository used to interact with account data storage.</param>
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Creates a new bank account with the specified account number, PIN code, and initial balance.
        /// </summary>
        /// <param name="accountNumber">The unique identifier for the new bank account.</param>
        /// <param name="pinCode">The PIN code for account security.</param>
        /// <param name="initialBalance">The initial balance to set for the new account.</param>
        /// <returns>A <see cref="BankAccount"/> object representing the newly created account.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the account number or PIN code is empty, or if the initial balance is negative.
        /// </exception>
        public BankAccount CreateAccount(string accountNumber, string pinCode, decimal initialBalance)
        {
            // Validate input values
            if (string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(pinCode))
            {
                throw new InvalidOperationException("Account number and PIN code cannot be empty.");
            }

            BankAccount newAccount = new BankAccount
            {
                AccountNumber = accountNumber,
                PinCode = pinCode,
                Balance = initialBalance
            };

            // Adds the account to the repository
            _accountRepository.AddAccount(newAccount);
            return newAccount;
        }

        /// <summary>
        /// Withdraws a specified amount from the bank account after validating the account number and PIN code.
        /// </summary>
        /// <param name="accountNumber">The unique identifier for the bank account from which to withdraw.</param>
        /// <param name="pinCode">The PIN code associated with the account to authenticate the transaction.</param>
        /// <param name="amount">The amount to withdraw from the account.</param>
        /// <returns>The updated balance of the account after the withdrawal.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the account is not found, or if there are insufficient funds for the withdrawal.
        /// </exception>
        public decimal Withdraw(string accountNumber, string pinCode, decimal amount)
        {
            BankAccount? account = _accountRepository.GetAccount(accountNumber, pinCode);

            // Check if the account does exist
            if (account == null)
            {
                throw new InvalidOperationException("Invalid account number or PIN.");
            }
                
            // Check if there are sufficient funds for the withdrawal
            if (!HasSufficientBalance(account, amount))
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
                
            account.Balance -= amount;

            _accountRepository.UpdateAccount(account);

            return account.Balance;
        }

        /// <summary>
        /// Checks if the given account has enough balance for a specified withdrawal amount.
        /// </summary>
        /// <param name="account">The bank account to check.</param>
        /// <param name="amount">The amount to compare against the account balance.</param>
        /// <returns><c>true</c> if the account has sufficient balance; otherwise, <c>false</c>.</returns>
        public bool HasSufficientBalance(BankAccount account, decimal amount)
        {
            
            return account.Balance >= amount;
        }
    }
}
