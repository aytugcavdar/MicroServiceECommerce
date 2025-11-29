using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Commands.Create;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    // ARTIK GENERIC DEĞİL, SENİN INTERFACE'İN
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = new Product(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.PictureFileName,
            request.CategoryId
        );

        // Yine aynı metodları kullanabilirsin çünkü IProductRepository, IAsyncRepository'den miras aldı.
        await _productRepository.AddAsync(product);

        return product.Id;
    }
}
