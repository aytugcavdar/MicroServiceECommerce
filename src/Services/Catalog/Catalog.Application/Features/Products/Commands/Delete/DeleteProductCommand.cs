using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Commands.Delete;

public class DeleteProductCommand : IRequest<DeleteProductCommandResponse>
{
    public Guid Id { get; set; }

    public DeleteProductCommand(Guid id)
    {
        Id = id;
    }
}
