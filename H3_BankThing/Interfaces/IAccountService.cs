using H3_BankThing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Interfaces
{
    public interface IAccountService
    {
        BankAccount CreateAccount(string accountNumber, string pinCode, decimal initialBalance);
        decimal Withdraw(string accountNumber, string pinCode, decimal amount);
    }

}
