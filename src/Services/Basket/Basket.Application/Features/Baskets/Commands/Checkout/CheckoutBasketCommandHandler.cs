using Basket.Application.Services;
using BuildingBlocks.CrossCutting.Exceptions.types;
using BuildingBlocks.Messaging.IntegrationEvents;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Basket.Application.Features.Baskets.Commands.Checkout;

public class CheckoutBasketCommandHandler : IRequestHandler<CheckoutBasketCommand, CheckoutBasketCommandResponse>
{
    private readonly IBasketRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CheckoutBasketCommandHandler> _logger;

    public CheckoutBasketCommandHandler(
        IBasketRepository repository,
        IPublishEndpoint publishEndpoint,
        ILogger<CheckoutBasketCommandHandler> logger)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<CheckoutBasketCommandResponse> Handle(CheckoutBasketCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("🛒 Checkout started for user: {UserName}", request.UserName);

        var basket = await _repository.GetBasketAsync(request.UserName, cancellationToken);

        if (basket == null || basket.IsEmpty)
        {
            throw new BusinessException("Basket is empty or not found");
        }

        var eventMessage = new BasketCheckoutEvent
        {
            UserName = request.UserName,
            TotalPrice = basket.TotalPrice,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailAddress = request.EmailAddress,
            AddressLine = request.AddressLine,
            Country = request.Country,
            State = request.State,
            ZipCode = request.ZipCode,
            CardName = request.CardName,
            CardNumber = request.CardNumber,
            Expiration = request.Expiration,
            CVV = request.CVV,
            BuyerId = request.UserName
        };

        await _publishEndpoint.Publish(eventMessage, cancellationToken);
        _logger.LogInformation("📤 BasketCheckoutEvent published for user: {UserName}", request.UserName);

        await _repository.DeleteBasketAsync(basket.UserName, cancellationToken);
        _logger.LogInformation("🗑️ Basket cleared for user: {UserName}", request.UserName);

        return new CheckoutBasketCommandResponse
        {
            Success = true,
            Message = "Checkout completed successfully"
        };
    }
}