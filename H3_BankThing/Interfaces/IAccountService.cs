using H3_BankThing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Interfaces
{
    /// <summary>
    /// Defines the contract for account-related services, including account creation and withdrawal.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Creates a new bank account with the specified account number, PIN code, and initial balance.
        /// </summary>
        /// <param name="accountNumber">The unique identifier for the new bank account.</param>
        /// <param name="pinCode">The PIN code for account security.</param>
        /// <param name="initialBalance">The initial balance to be set for the account.</param>
        /// <returns>A <see cref="BankAccount"/> object representing the newly created account.</returns>
        BankAccount CreateAccount(string accountNumber, string pinCode, decimal initialBalance);

        /// <summary>
        /// Withdraws a specified amount from an account after validating the account number and PIN code.
        /// </summary>
        /// <param name="accountNumber">The unique identifier of the bank account from which to withdraw.</param>
        /// <param name="pinCode">The PIN code associated with the account to verify the transaction.</param>
        /// <param name="amount">The amount to be withdrawn from the account.</param>
        /// <returns>The updated balance of the account after the withdrawal.</returns>
        decimal Withdraw(string accountNumber, string pinCode, decimal amount);
    }

}
