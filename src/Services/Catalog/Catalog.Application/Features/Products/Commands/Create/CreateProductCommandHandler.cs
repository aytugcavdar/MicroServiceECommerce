using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Commands.Create;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CreateProductCommandResponse>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<CreateProductCommandResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.PictureFileName,
            request.CategoryId
        );

        await _productRepository.AddAsync(product);

        await _productRepository.SaveChangesAsync(cancellationToken);

        return new CreateProductCommandResponse(
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.CreatedDate
        );
    }
}
