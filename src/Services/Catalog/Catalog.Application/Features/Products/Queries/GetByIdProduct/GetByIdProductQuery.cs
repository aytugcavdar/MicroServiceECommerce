
using MediatR;
using System;

namespace Catalog.Application.Features.Products.Queries.GetByIdProduct;

public class GetByIdProductQuery : IRequest<GetByIdProductDto>
{
    public Guid Id { get; set; }

    public GetByIdProductQuery(Guid id)
    {
        Id = id;
    }
}
