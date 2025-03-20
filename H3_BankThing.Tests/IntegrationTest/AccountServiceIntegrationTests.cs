using H3_BankThing.Data;
using H3_BankThing.Models;
using H3_BankThing.Repositories;
using H3_BankThing.Services;
using H3_BankThing.Tests.Factories;
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
            // Generate a fake account
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount();

            _accountService.CreateAccount(fakeAccount.AccountNumber, fakeAccount.PinCode, fakeAccount.Balance);

            BankAccount? retrievedAccount = _repository.GetAccount(fakeAccount.AccountNumber, fakeAccount.PinCode);

            Assert.NotNull(retrievedAccount);
            Assert.Equal(fakeAccount.AccountNumber, retrievedAccount.AccountNumber);
            Assert.Equal(fakeAccount.PinCode, retrievedAccount.PinCode);
            Assert.Equal(fakeAccount.Balance, retrievedAccount.Balance);
        }

        /// <summary>
        /// Tests whether withdrawal works correctly when the account has sufficient balance.
        /// </summary>
        [Fact]
        public void Can_Withdraw_With_Sufficient_Balance()
        {
            // Generate a fake account with at least 500 balance
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount(balanceOverride: 500);

            _accountService.CreateAccount(fakeAccount.AccountNumber, fakeAccount.PinCode, fakeAccount.Balance);

            decimal newBalance = _accountService.Withdraw(fakeAccount.AccountNumber, fakeAccount.PinCode, 200);

            BankAccount? updatedAccount = _repository.GetAccount(fakeAccount.AccountNumber, fakeAccount.PinCode);

            Assert.NotNull(updatedAccount);
            Assert.Equal(fakeAccount.Balance - 200, updatedAccount.Balance);
        }

        /// <summary>
        /// Tests whether an incorrect PIN prevents a withdrawal.
        /// </summary>
        [Fact]
        public void Cannot_Withdraw_With_Incorrect_PIN()
        {
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount(balanceOverride: 500);

            _accountService.CreateAccount(fakeAccount.AccountNumber, fakeAccount.PinCode, fakeAccount.Balance);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.Withdraw(fakeAccount.AccountNumber, "9999", 200)); // Fake wrong PIN

            Assert.Equal("Invalid account number or PIN.", exception.Message);
        }

        /// <summary>
        /// Tests whether an incorrect account number prevents a withdrawal.
        /// </summary>
        [Fact]
        public void Cannot_Withdraw_With_Incorrect_Account_Number()
        {
            BankAccount fakeAccount = BankAccountFactory.NewFakeAccount(balanceOverride: 500);

            _accountService.CreateAccount(fakeAccount.AccountNumber, fakeAccount.PinCode, fakeAccount.Balance);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.Withdraw("00000000", fakeAccount.PinCode, 200)); // Fake wrong account number

            Assert.Equal("Invalid account number or PIN.", exception.Message);
        }
    }
}
