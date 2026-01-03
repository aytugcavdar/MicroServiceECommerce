using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Order.Application.Features.Orders.Commands;

public class CreateOrderCommand : IRequest<CreateOrderCommandResponse>
{
    public Guid UserId { get; set; }
    public AddressDto Address { get; set; }
    public List<CreateOrderItemDto> OrderItems { get; set; }

    public CreateOrderCommand()
    {
        OrderItems = new List<CreateOrderItemDto>();
    }
}

public class CreateOrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}

public class AddressDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ZipCode { get; set; }
}