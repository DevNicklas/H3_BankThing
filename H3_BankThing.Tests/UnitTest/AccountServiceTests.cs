using H3_BankThing.Interfaces;
using H3_BankThing.Models;
using H3_BankThing.Services;
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
    /// Unit tests for the <see cref="AccountService"/> class, testing account creation, withdrawal, 
    /// and handling of various edge cases such as insufficient balance and incorrect account credentials.
    /// </summary>
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockRepository;
        private readonly AccountService _accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountServiceTests"/> class and 
        /// sets up the necessary mock repository and account service instance.
        /// </summary>
        public AccountServiceTests()
        {
            _mockRepository = new Mock<IAccountRepository>();

            // Initialize AccountService with the mocked repository
            _accountService = new AccountService(_mockRepository.Object);
        }

        /// <summary>
        /// Tests the ability to successfully create a new account with valid parameters.
        /// </summary>
        [Fact]
        public void Can_Create_Account()
        {
            string accountNumber = "12345678";
            string pinCode = "1234";
            decimal initialBalance = 1000;

            BankAccount account = _accountService.CreateAccount(accountNumber, pinCode, initialBalance);

            // Assert that the account was created correctly
            Assert.NotNull(account);
            Assert.Equal(accountNumber, account.AccountNumber);
            Assert.Equal(pinCode, account.PinCode);
            Assert.Equal(initialBalance, account.Balance);
        }

        /// <summary>
        /// Tests that an exception is thrown when attempting to create an account without an account number.
        /// </summary>
        [Fact]
        public void Cannot_Create_Account_Without_Account_Number()
        {
            // Assert that an exception is thrown when the account number is empty
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.CreateAccount("", "1234", 100));

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
                _accountService.CreateAccount("12345678", "", 100));

            // Assert the exception message
            Assert.Equal("Account number and PIN code cannot be empty.", exception.Message);
        }

        /// <summary>
        /// Tests the ability to withdraw money from an account when sufficient balance is available.
        /// </summary>
        [Fact]
        public void Can_Withdraw_With_Sufficient_Balance()
        {
            BankAccount account = _accountService.CreateAccount("12345678", "1234", 500);

            // Setup mock repository to return the created account
            _mockRepository.Setup(repo => repo.GetAccount("12345678", "1234")).Returns(account);

            decimal newBalance = _accountService.Withdraw("12345678", "1234", 200);

            // Assert the updated balance is 300
            Assert.Equal(300, account.Balance);
        }

        /// <summary>
        /// Tests that an exception is thrown when attempting to withdraw money with insufficient balance.
        /// </summary>
        [Fact]
        public void Cannot_Withdraw_With_Insufficient_Balance()
        {
            BankAccount account = _accountService.CreateAccount("12345678", "1234", 100);

            // Setup mock repository to return the created account
            _mockRepository.Setup(repo => repo.GetAccount("12345678", "1234")).Returns(account);

            // Assert that an exception is thrown when attempting to withdraw 200 (insufficient funds)
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.Withdraw("12345678", "1234", 200));

            // Assert the exception message and verify balance remains unchanged
            Assert.Equal("Insufficient funds.", exception.Message);
            Assert.Equal(100, account.Balance);
        }

        /// <summary>
        /// Tests that an exception is thrown when attempting to withdraw from an account with an incorrect PIN code.
        /// </summary>
        [Fact]
        public void Cannot_Withdraw_With_Incorrect_Pin()
        {
            BankAccount account = _accountService.CreateAccount("12345678", "1234", 500);

            // Setup mock repository to return null for the incorrect PIN code
            _mockRepository.Setup(repo => repo.GetAccount("12345678", "0000")).Returns((BankAccount?)null);

            // Assert that an exception is thrown for incorrect PIN
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.Withdraw("12345678", "0000", 200));

            // Assert the exception message and verify balance remains unchanged
            Assert.Equal("Invalid account number or PIN.", exception.Message);
            Assert.Equal(500, account.Balance);
        }

        /// <summary>
        /// Tests that an exception is thrown when attempting to withdraw from an account with an incorrect account number.
        /// </summary>
        [Fact]
        public void Cannot_Withdraw_With_Incorrect_Account_Number()
        {
            BankAccount account = _accountService.CreateAccount("12345678", "1234", 500);

            // Setup mock repository to return null for the incorrect account number
            _mockRepository.Setup(repo => repo.GetAccount("00000000", "1234")).Returns((BankAccount?)null);

            // Assert that an exception is thrown for incorrect account number
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.Withdraw("00000000", "1234", 200));

            // Assert the exception message and verify balance remains unchanged
            Assert.Equal("Invalid account number or PIN.", exception.Message);
            Assert.Equal(500, account.Balance);
        }

    }
}
