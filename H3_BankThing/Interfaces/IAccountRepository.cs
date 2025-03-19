using H3_BankThing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Interfaces
{
    public interface IAccountRepository
    {
        BankAccount GetAccount(string accountNumber, string pinCode);
        void SaveAccount(BankAccount account);
        void UpdateAccount(BankAccount account);
    }
}
