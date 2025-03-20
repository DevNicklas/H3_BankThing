using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using H3_BankThing.Models;

namespace H3_BankThing.Tests.Factories
{
    /// <summary>
    /// Factory class for generating fake <see cref="BankAccount"/> objects for testing purposes.
    /// Uses the Bogus library to create realistic test data.
    /// </summary>
    public static class BankAccountFactory
    {
        private static readonly Faker _faker = new Faker();

        /// <summary>
        /// Creates a new fake bank account with randomly generated or overridden values.
        /// </summary>
        /// <param name="accountNumberOverride">Optional custom account number. If not provided, a random one is generated.</param>
        /// <param name="pinCodeOverride">Optional custom PIN code. If not provided, a random one is generated.</param>
        /// <param name="balanceOverride">Optional custom balance. If not provided, a random balance between 100 and 5000 is generated.</param>
        /// <returns>A new instance of <see cref="BankAccount"/> with randomized or overridden data.</returns>
        public static BankAccount NewFakeAccount(string? accountNumberOverride = null, string? pinCodeOverride = null, decimal? balanceOverride = null)
        {
            return new BankAccount
            {
                AccountNumber = accountNumberOverride ?? GenerateAccountNumber(),
                PinCode = pinCodeOverride ?? GeneratePinCode(),
                Balance = balanceOverride ?? _faker.Random.Decimal(100, 5000)
            };
        }

        /// <summary>
        /// Generates a random 8-digit bank account number.
        /// </summary>
        /// <returns>A randomly generated account number as a string.</returns>
        private static string GenerateAccountNumber()
        {
            return _faker.Random.ReplaceNumbers("########");
        }

        /// <summary>
        /// Generates a random 4-digit PIN code.
        /// </summary>
        /// <returns>A randomly generated PIN code as a string.</returns>
        private static string GeneratePinCode()
        {
            return _faker.Random.ReplaceNumbers("####");
        }
    }
}
