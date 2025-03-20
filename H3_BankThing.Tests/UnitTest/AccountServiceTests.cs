using H3_BankThing.Data;
using H3_BankThing.Interfaces;
using H3_BankThing.Models;
using H3_BankThing.Repositories;
using H3_BankThing.Services;
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
            // Assert that an exception is thrown when the account number is empty
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.CreateAccount("", "1234", 1000));

            // Assert the exception message
            Assert.Equal("Account number and PIN code cannot be empty.", exception.Message);
        }

        /// <summary>
        /// Tests that an exception is thrown when attempting to create an account without a PIN code.
        /// </summary>
        [Fact]
        public void Cannot_Create_Account_Without_PIN()
        {
            // Assert that an exception is thrown when the PIN code is empty
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.CreateAccount("12345678", "", 1000));

            // Assert the exception message
            Assert.Equal("Account number and PIN code cannot be empty.", exception.Message);
        }

        /// <summary>
        /// Tests that <see cref="AccountService.HasSufficientBalance"/> returns true 
        /// when the account has enough balance for the requested withdrawal.
        /// </summary>
        [Fact]
        public void Returns_True_When_Balance_Is_Sufficient()
        {
            BankAccount account = new BankAccount
            {
                AccountNumber = "12345678",
                PinCode = "1234",
                Balance = 500
            };

            // Assert that the method returns true for a withdrawal of 300
            Assert.True(_accountService.HasSufficientBalance(account, 300));
        }

        /// <summary>
        /// Tests that <see cref="AccountService.HasSufficientBalance"/> returns false 
        /// when the account does not have enough balance for the requested withdrawal.
        /// </summary>
        [Fact]
        public void Returns_False_When_Balance_Is_InSufficient()
        {
            BankAccount account = new BankAccount
            {
                AccountNumber = "12345678",
                PinCode = "1234",
                Balance = 200
            };

            // Assert that the method returns false for a withdrawal of 300
            Assert.False(_accountService.HasSufficientBalance(account, 300));
        }
    }
}
