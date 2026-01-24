
using AutoMapper;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.CrossCutting.Exceptions.types;

namespace Catalog.Application.Features.Products.Queries.GetByIdProduct;

public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQuery, GetByIdProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetByIdProductQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<GetByIdProductDto> Handle(GetByIdProductQuery request, CancellationToken cancellationToken)
    {

        var product = await _productRepository.GetAsync(
            predicate: p => p.Id == request.Id,
            include: p => p.Include(c => c.Category),
            cancellationToken: cancellationToken
        );

        if (product == null)
            throw new BusinessException("Product not found"); 

        
        return new GetByIdProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.PictureFileName,
            CategoryName = product.Category?.Name ?? "Uncategorized"
        };
    }
}
