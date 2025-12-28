using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Basket.Application.Features.Baskets.Commands.UpdateBasket;

public class UpdateBasketCommand : IRequest<UpdateBasketCommandResponse>
{
    public string UserName { get; set; } = string.Empty;
    public List<UpdateBasketItemDto> Items { get; set; } = new();
}
