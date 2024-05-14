using System.Linq.Expressions;
using Application.Interfaces;
using Application.Services;
using AutoFixture;
using Core.Entities;
using Core.Entities.SpecificData;
using Core.Interfaces;
using Moq;
using NUnit.Framework;
using Assert = Xunit.Assert;

namespace Tests.Services;

[TestFixture]
[TestOf(typeof(AccountService))]
public class AccountServiceTest
{
    private readonly Fixture _fixture = MyFixture.Create();
    private readonly AccountService _accountService;
    private readonly BrokerageAccount _account;

    private readonly Bond _bond = new()
    {
        Nominal = new MoneyValue
        {
            Units = 1,
            Currency = new CurrencyCode("rub")
        }
    };

    public AccountServiceTest()
    {
        _account = _fixture.Create<BrokerageAccount>();
        // var unitOfWorkMock = Mock.Of<IUnitOfWork>(u => u.Accounts.GetAsync(
        //     It.IsAny<Expression<Func<BrokerageAccount, bool>>>(),
        //     It.IsAny<Expression<Func<BrokerageAccount, object>>[]>(),
        //     It.IsAny<bool>()) == Task.FromResult(_account));
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(u => u.Accounts
                .GetAsync(It.IsAny<Expression<Func<BrokerageAccount, bool>>>(),
                    It.IsAny<Expression<Func<BrokerageAccount, object>>[]>(),
                    It.IsAny<bool>()))
            .ReturnsAsync(_account);
        unitOfWorkMock.Setup(u => u.Currencies.Update(It.IsAny<StockContainer<Currency>>()));
        unitOfWorkMock.Setup(u => u.Currencies.Remove(It.IsAny<StockContainer<Currency>>()));
        
        var stockExchangeMock = new Mock<IStockExchangeService>();
        stockExchangeMock.Setup(s => s
                .GetBondByUidAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_bond);
        _accountService = new AccountService(unitOfWorkMock.Object, stockExchangeMock.Object);
    }

    [Test]
    public async Task BuyStock_Bond()
    {
        // Arrange
        _account.Currencies.Add(new StockContainer<Currency>(
            new Currency
            {
                Nominal = new MoneyValue
                {
                    Units = 1, 
                    Currency = new CurrencyCode("rub")
                },
                Lot = 1000,
            }, 10));
        
        // Act
        var result = await _accountService.BuyStock<Bond>(
            _account.OwnerId,
            Guid.NewGuid(),
            _fixture.Create<string>(),
            _fixture.Create<ulong>());
        
        // Assert
        Assert.Equal(_bond, result);
    }
}