using H3_BankThing.Data;
using H3_BankThing.Interfaces;
using H3_BankThing.Models;
using H3_BankThing.Repositories;
using H3_BankThing.Services;
using H3_BankThing.Tests.Factories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

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
        /// Initializes a new instance of the <see cref="AccountServiceTests"/> class 
        /// and sets up a mock repository and account service instance.
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

            // Assert that an exception is thrown when the account number is empty
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.CreateAccount(fakeAccount.AccountNumber, fakeAccount.PinCode, fakeAccount.Balance));

            // Assert the exception message
            Assert.Equal("Account number cannot be empty.", exception.Message);
        }

        /// <summary>
        /// Tests that an exception is thrown when attempting to create an account with an invalid PIN code.
        /// </summary>
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

            // Assert that an exception is thrown when the PIN is invalid
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.CreateAccount(fakeAccount.AccountNumber, fakeAccount.PinCode, fakeAccount.Balance));

            // Assert the exception message
            Assert.Equal("PIN code must be exactly 4 digits long and contain only numbers.", exception.Message);
        }

        /// <summary>
        /// Tests that <see cref="AccountService.HasSufficientBalance"/> returns true 
        /// when the account has enough balance for the requested withdrawal.
        /// </summary>
        [Fact]
        public void Returns_True_When_Balance_Is_Sufficient()
        {
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount(balanceOverride: 500);

            // Assert that the method returns true for a withdrawal of 300
            Assert.True(_accountService.HasSufficientBalance(fakeAccount, 300));
        }

        /// <summary>
        /// Tests that <see cref="AccountService.HasSufficientBalance"/> returns false 
        /// when the account does not have enough balance for the requested withdrawal.
        /// </summary>
        [Fact]
        public void Returns_False_When_Balance_Is_InSufficient()
        {
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount(balanceOverride: 200);

            // Assert that the method returns false for a withdrawal of 300
            Assert.False(_accountService.HasSufficientBalance(fakeAccount, 300));
        }
    }
}
