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

namespace H3_BankThing.Tests
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockRepository;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _mockRepository = new Mock<IAccountRepository>();
            _accountService = new AccountService(_mockRepository.Object);
        }

        [Fact]
        public void Can_Create_Account()
        {
            string accountNumber = "1234";
            string pinCode = "5678";
            decimal initialBalance = 1000;

            BankAccount account = _accountService.CreateAccount(accountNumber, pinCode, initialBalance);

            Assert.NotNull(account);
            Assert.Equal(accountNumber, account.AccountNumber);
            Assert.Equal(pinCode, account.PinCode);
            Assert.Equal(initialBalance, account.Balance);
        }

        [Fact]
        public void Cannot_Create_Account_Without_Account_Number()
        {
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.CreateAccount("", "1234", 100));

            Assert.Equal("Account number and PIN code cannot be empty.", exception.Message);
        }

        [Fact]
        public void Cannot_Create_Account_Without_PIN()
        {
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.CreateAccount("12345678", "", 100));

            Assert.Equal("Account number and PIN code cannot be empty.", exception.Message);
        }

        [Fact]
        public void Can_Withdraw_With_Sufficient_Balance()
        {
            BankAccount account = new BankAccount
            {
                AccountNumber = "12345678",
                PinCode = "1234",
                Balance = 500
            };

            _mockRepository.Setup(repo => repo.GetAccount("12345678", "1234")).Returns(account);

            decimal newBalance = _accountService.Withdraw("12345678", "1234", 200);

            Assert.Equal(300, newBalance);
            _mockRepository.Verify(repo => repo.UpdateAccount(It.Is<BankAccount>(a => a.Balance == 300)), Times.Once);
        }

        [Fact]
        public void Cannot_Withdraw_With_Insufficient_Balance()
        {
            BankAccount account = new BankAccount
            {
                AccountNumber = "12345678",
                PinCode = "1234",
                Balance = 100
            };

            _mockRepository.Setup(repo => repo.GetAccount("12345678", "1234")).Returns(account);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                _accountService.Withdraw("12345678", "1234", 200));

            Assert.Equal("Insufficient funds.", exception.Message);

            Assert.Equal(100, account.Balance);
            _mockRepository.Verify(repo => repo.UpdateAccount(It.IsAny<BankAccount>()), Times.Never);
        }
    }
}
