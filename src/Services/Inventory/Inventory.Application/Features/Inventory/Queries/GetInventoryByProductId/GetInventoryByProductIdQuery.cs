using MediatR;

namespace Inventory.Application.Features.Inventory.Queries.GetInventoryByProductId;

public class GetInventoryByProductIdQuery : IRequest<GetInventoryByProductIdResponse>
{
    public Guid ProductId { get; set; }
}
