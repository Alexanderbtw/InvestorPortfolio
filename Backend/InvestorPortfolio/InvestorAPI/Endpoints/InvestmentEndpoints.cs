using System.Security.Claims;
using Application.Services;
using Core.Entities;
using Core.Interfaces;
using InvestorAPI.Contracts.Investment;

namespace InvestorAPI.Endpoints;

public static class InvestmentEndpoints
{
    public static RouteGroupBuilder MapPublicInvestmentEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/shares",
            async (IStockExchangeService stockExchange, CancellationToken cancellationToken) =>
                await stockExchange.GetSharesAsync(cancellationToken));
        group.MapGet("/currencies",
            async (IStockExchangeService stockExchange, CancellationToken cancellationToken) =>
                await stockExchange.GetCurrenciesAsync(cancellationToken));
        group.MapGet("/bonds",
            async (IStockExchangeService stockExchange, CancellationToken cancellationToken) =>
                await stockExchange.GetBondsAsync(cancellationToken));
        group.MapGet("/shares/{uid}",
            async (IStockExchangeService stockExchange, string uid, CancellationToken cancellationToken) =>
                await stockExchange.GetBondByUidAsync(uid, cancellationToken));
        group.MapGet("/currencies/{uid}",
            async (IStockExchangeService stockExchange, string uid, CancellationToken cancellationToken) =>
                await stockExchange.GetCurrencyByUidAsync(uid, cancellationToken));
        group.MapGet("/bonds/{uid}",
            async (IStockExchangeService stockExchange, string uid, CancellationToken cancellationToken) =>
                await stockExchange.GetBondByUidAsync(uid, cancellationToken));

        return group;
    }

    public static RouteGroupBuilder MapIndividualInvestmentEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/portfolio", GetPortfolioInfo);
        group.MapGet("/account/{accountId:guid}", GetAccountInfo);
        group.MapPost("/account", CreateAccount);
        group.MapPatch("/topup", TopUpAccount);
        group.MapPatch("/buy/share", BuyShare);
        group.MapPatch("buy/bond", BuyBond);
        group.MapPatch("buy/currency", BuyCurrency);
        group.MapPut("sell/share", SellShare);
        group.MapPut("sell/bond", SellBond);
        group.MapPut("sell/currency", SellCurrency);

        return group;
    }

    private static async Task<IResult> SellCurrency(
        BuyStockRequest request,
        ClaimsPrincipal user,
        AccountService accountService)
    {
        var id = Guid.Parse(user.Identity!.Name!);

        var res = await accountService.SellStock<Currency>(id, request.AccountId, request.Uid, request.Quantity);
        return Results.Ok(res);
    }

    private static async Task<IResult> SellBond(
        BuyStockRequest request,
        ClaimsPrincipal user,
        AccountService accountService)
    {
        var id = Guid.Parse(user.Identity!.Name!);

        var res = await accountService.SellStock<Bond>(id, request.AccountId, request.Uid, request.Quantity);
        return Results.Ok(res);
    }

    private static async Task<IResult> SellShare(
        BuyStockRequest request,
        ClaimsPrincipal user,
        AccountService accountService)
    {
        var id = Guid.Parse(user.Identity!.Name!);

        var res = await accountService.SellStock<Share>(id, request.AccountId, request.Uid, request.Quantity);
        return Results.Ok(res);
    }

    private static async Task<IResult> BuyCurrency(
        BuyStockRequest request,
        ClaimsPrincipal user,
        AccountService accountService)
    {
        var id = Guid.Parse(user.Identity!.Name!);

        var res = await accountService.BuyStock<Currency>(id, request.AccountId, request.Uid, request.Quantity);
        return Results.Ok(res);
    }

    private static async Task<IResult> BuyBond(
        BuyStockRequest request,
        ClaimsPrincipal user,
        AccountService accountService)
    {
        var id = Guid.Parse(user.Identity!.Name!);

        var res = await accountService.BuyStock<Bond>(id, request.AccountId, request.Uid, request.Quantity);
        return Results.Ok(res);
    }

    private static async Task<IResult> BuyShare(
        BuyStockRequest request,
        ClaimsPrincipal user,
        AccountService accountService)
    {
        var id = Guid.Parse(user.Identity!.Name!);

        var res = await accountService.BuyStock<Share>(id, request.AccountId, request.Uid, request.Quantity);
        return Results.Ok(res);
    }

    private static async Task<IResult> TopUpAccount(
        TopupAccountRequest request,
        ClaimsPrincipal user,
        AccountService accountService)
    {
        await accountService.TopUpAccount(Guid.Parse(user.Identity!.Name!), request.AccountId, request.Value);

        return Results.Ok("Top up successful");
    }
    
    private static async Task<IResult> CreateAccount(
        CreateAccountRequest request,
        ClaimsPrincipal user,
        PortfolioService portfolioService)
    {
        var id = Guid.Parse(user.Identity!.Name!);
        try
        {
            var res = await portfolioService.AddAccountAsync(id, request.Name);
            return Results.Ok(new CreateAccountResponse(res));
        }
        catch (Exception e)
        {
            return Results.BadRequest(e.Message);
        }
    }

    private static async Task<IResult> GetPortfolioInfo(
        ClaimsPrincipal user,
        PortfolioService portfolioService)
    {
        try
        {
            var portfolio = await portfolioService.GetPortfolioByIdAsync(Guid.Parse(user.Identity!.Name!));
            return Results.Ok(portfolio);
        }
        catch (Exception e)
        {
            return Results.NotFound(e.Message);
        }
    }

    private static async Task<IResult> GetAccountInfo(
        Guid accountId,
        ClaimsPrincipal user,
        AccountService accountService)
    {
        try
        {
            var account = await accountService.GetAccountByIdAsync(Guid.Parse(user.Identity!.Name!), accountId);
            return Results.Ok(account);
        }
        catch (Exception e)
        {
            return Results.NotFound(e.Message);
        }
    }
}