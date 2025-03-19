using H3_BankThing.Interfaces;
using H3_BankThing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public BankAccount CreateAccount(string accountNumber, string pinCode, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(accountNumber) || string.IsNullOrWhiteSpace(pinCode))
            {
                throw new InvalidOperationException("Account number and PIN code cannot be empty.");
            }

            if (initialBalance < 0)
            {
                throw new InvalidOperationException("Initial balance cannot be negative.");
            }

            BankAccount newAccount = new BankAccount
            {
                AccountNumber = accountNumber,
                PinCode = pinCode,
                Balance = initialBalance
            };

            _accountRepository.SaveAccount(newAccount);
            return newAccount;
        }

        public decimal Withdraw(string accountNumber, string pinCode, decimal amount)
        {
            BankAccount account = _accountRepository.GetAccount(accountNumber, pinCode);

            if (account == null)
            {
                throw new InvalidOperationException("Invalid account number or PIN.");
            }
                

            if (account.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
                
            account.Balance -= amount;

            return account.Balance;
        }
    }
}
