using H3_BankThing.Data;
using H3_BankThing.Models;
using H3_BankThing.Repositories;
using H3_BankThing.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H3_BankThing.Tests.IntegrationTest
{
    /// <summary>
    /// Integration tests for the <see cref="AccountService"/> class, ensuring database interactions 
    /// and business logic function correctly.
    /// </summary>
    public class AccountServiceIntegrationTests
    {
        private readonly ApplicationDbContext _context;
        private readonly AccountService _accountService;
        private readonly AccountRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountServiceIntegrationTests"/> class 
        /// and sets up an in-memory database for testing.
        /// </summary>
        public AccountServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "BankTestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new AccountRepository(_context);
            _accountService = new AccountService(_repository);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        /// <summary>
        /// Tests whether an account can be created and successfully retrieved from the database.
        /// </summary>
        [Fact]
        public void Can_Create_Account_And_Retrieve_From_Database()
        {
            string accountNumber = "12345678";
            string pinCode = "1234";
            decimal initialBalance = 1000;

            _accountService.CreateAccount(accountNumber, pinCode, initialBalance);

            BankAccount? retrievedAccount = _repository.GetAccount(accountNumber, pinCode);

            Assert.NotNull(retrievedAccount);
            Assert.Equal(accountNumber, retrievedAccount.AccountNumber);
            Assert.Equal(pinCode, retrievedAccount.PinCode);
            Assert.Equal(initialBalance, retrievedAccount.Balance);
        }

        /// <summary>
        /// Tests whether withdrawal works correctly when the account has sufficient balance.
        /// </summary>
        [Fact]
        public void Can_Withdraw_With_Sufficient_Balance()
        {
            string accountNumber = "12345678";
            string pinCode = "1234";
            decimal initialBalance = 500;

            _accountService.CreateAccount(accountNumber, pinCode, initialBalance);

            decimal newBalance = _accountService.Withdraw(accountNumber, pinCode, 200);

            BankAccount? updatedAccount = _repository.GetAccount(accountNumber, pinCode);

            Assert.NotNull(updatedAccount);
            Assert.Equal(300, updatedAccount.Balance);
        }

        /// <summary>
        /// Tests whether an incorrect PIN prevents a withdrawal.
        /// </summary>
        [Fact]
        public void Cannot_Withdraw_With_Incorrect_PIN()
        {
            _accountService.CreateAccount("12345678", "1234", 500);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
            _accountService.Withdraw("12345678", "0000", 200));

            Assert.Equal("Invalid account number or PIN.", exception.Message);
        }

        /// <summary>
        /// Tests whether an incorrect account number prevents a withdrawal.
        /// </summary>
        [Fact]
        public void Cannot_Withdraw_With_Incorrect_Account_Number()
        {
            _accountService.CreateAccount("12345678", "1234", 500);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
            _accountService.Withdraw("", "1234", 200));

            Assert.Equal("Invalid account number or PIN.", exception.Message);
        }
    }
}
