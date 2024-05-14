using AutoFixture;
using Core.Entities;
using Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Persistence;
using Persistence.Repositories;
using Xunit;

namespace Tests
{
    public class RepositoriesTests : IDisposable, IAsyncDisposable
    {
        private readonly Fixture _fixture = MyFixture.Create();
        private readonly InvestmentDbContext _context;
        
        public RepositoriesTests()
        {
            var options = new DbContextOptionsBuilder<InvestmentDbContext>()
                .UseInMemoryDatabase(databaseName: "InvestmentDbTests")
                .Options;
            
            _context = new InvestmentDbContext(options);
        }
        
        [Fact]
        public async Task PortfolioRepository_CanAddPortfolio()
        {
            // Arrange
            var repo = new PortfolioRepository(_context, new NullLoggerFactory());

            // Act
            var result = await repo.TryAddAsync(_fixture.Create<Portfolio>());
            await _context.SaveChangesAsync();

            // Assert
            Assert.True(result);
            Assert.Equal(1, _context.Portfolios.Count());
        }

        [Fact]
        public async Task PortfolioRepository_CanDeletePortfolio()
        {
            // Arrange
            var repo = new PortfolioRepository(_context, new NullLoggerFactory());
            var portfolioEntry = await _context.Portfolios.AddAsync(_fixture.Create<Portfolio>());
            await _context.SaveChangesAsync();
            
            // Act
            var result = repo.Remove(portfolioEntry.Entity);
            await _context.SaveChangesAsync();

            // Assert
            Assert.True(result);
            Assert.Equal(0, _context.Portfolios.Count());
        }

        [Fact]
        public async Task ShareRepository_CanAddStock()
        {
            // Arrange
            var repo = new ShareRepository(_context, new NullLoggerFactory());
            
            // Act
            var result = await repo.TryAddAsync(_fixture.Create<StockContainer<Share>>());
            await _context.SaveChangesAsync();
            
            // Assert
            Assert.True(result);
            Assert.Equal(1, _context.Shares.Count());
            
        }

        [Fact]
        public async Task ShareRepository_CanDeleteStock()
        {
            // Arrange
            var repo = new ShareRepository(_context, new NullLoggerFactory());
            var shareEntry = await _context.Shares.AddAsync(_fixture.Create<StockContainer<Share>>());
            await _context.SaveChangesAsync();
            
            // Act
            var result = await repo.TryAddAsync(_fixture.Create<StockContainer<Share>>());
            await _context.SaveChangesAsync();
            
            // Assert
            Assert.True(result);
            Assert.Equal(0, _context.Portfolios.Count());
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}