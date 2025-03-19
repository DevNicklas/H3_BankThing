using H3_BankThing.Interfaces;
using H3_BankThing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing
{
    public class AccountRepository : IAccountRepository
    {
        private readonly List<BankAccount> _accounts = new List<BankAccount>();

        public BankAccount GetAccount(string accountNumber, string pinCode)
        {
            return _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber && a.PinCode == pinCode);
        }

        public void SaveAccount(BankAccount account)
        {
            _accounts.Add(account);
        }

        public void UpdateAccount(BankAccount account)
        {
            BankAccount? existingAccount = _accounts.FirstOrDefault(a => a.AccountNumber == account.AccountNumber);
            if (existingAccount != null)
            {
                _accounts.Remove(existingAccount);
                _accounts.Add(account);
            }
        }
    }
}
