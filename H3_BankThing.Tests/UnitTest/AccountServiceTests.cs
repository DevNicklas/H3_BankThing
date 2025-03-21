using H3_BankThing.Data;
using H3_BankThing.Interfaces;
using H3_BankThing.Models;
using H3_BankThing.Repositories;
using H3_BankThing.Services;
using H3_BankThing.Tests.Factories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace H3_BankThing.Tests.UnitTest
{
    /// <summary>
    /// Unit tests for the <see cref="AccountService"/> class, covering account creation validation 
    /// and balance sufficiency checks.
    /// </summary>
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockRepository;
        private readonly AccountService _accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountServiceTests"/> class, 
        /// setting up a mock repository and an instance of <see cref="AccountService"/>.
        /// </summary>
        public AccountServiceTests()
        {
            _mockRepository = new Mock<IAccountRepository>();
            _accountService = new AccountService(_mockRepository.Object);
        }

        /// <summary>
        /// Tests that an exception is thrown when attempting to create an account without an account number.
        /// </summary>
        [Fact]
        public void Cannot_Create_Account_Without_Account_Number()
        {
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount(accountNumberOverride: "");

            // Assert that an exception is thrown with the expected message
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.CreateAccount(fakeAccount.AccountNumber, fakeAccount.PinCode, fakeAccount.Balance));

            Assert.Equal("Account number cannot be empty.", exception.Message);
        }

        /// <summary>
        /// Tests that an exception is thrown when attempting to create an account with an invalid PIN code.
        /// </summary>
        /// <param name="invalidPin">The invalid PIN code to test.</param>
        [Theory]
        [InlineData("123")]
        [InlineData("12345")]
        [InlineData("12a4")]
        [InlineData("abcd")]
        [InlineData("!@#$")]
        [InlineData(" 123")]
        public void Cannot_Create_Account_With_Invalid_PIN(string invalidPin)
        {
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount(pinCodeOverride: invalidPin);

            // Assert that an exception is thrown with the expected message
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.CreateAccount(fakeAccount.AccountNumber, fakeAccount.PinCode, fakeAccount.Balance));

            Assert.Equal("PIN code must be exactly 4 digits long and contain only numbers.", exception.Message);
        }

        /// <summary>
        /// Tests that the <see cref="AccountService.HasSufficientBalance"/> method returns true 
        /// when there is sufficient balance in the account for the requested withdrawal.
        /// </summary>
        [Fact]
        public void Returns_True_When_Balance_Is_Sufficient()
        {
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount(balanceOverride: 500);

            // Assert that the account has sufficient balance for a withdrawal of 300
            Assert.True(_accountService.HasSufficientBalance(fakeAccount, 300));
        }

        /// <summary>
        /// Tests that the <see cref="AccountService.HasSufficientBalance"/> method returns false 
        /// when there is insufficient balance in the account for the requested withdrawal.
        /// </summary>
        [Fact]
        public void Returns_False_When_Balance_Is_InSufficient()
        {
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount(balanceOverride: 200);

            // Assert that the account does not have sufficient balance for a withdrawal of 300
            Assert.False(_accountService.HasSufficientBalance(fakeAccount, 300));
        }
    }
}
