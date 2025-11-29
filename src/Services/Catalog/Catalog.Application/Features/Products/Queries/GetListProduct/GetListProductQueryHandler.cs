using AutoMapper;
using BuildingBlocks.Core.Paging;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Catalog.Application.Features.Products.Queries.GetListProduct;

public class GetListProductQueryHandler:IRequestHandler<GetListProductQuery, Paginate<GetListProductListItemDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetListProductQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Paginate<GetListProductListItemDto>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
    {
        // 1. Veritabanından sayfalı çek (Include ile Kategori adını da al)
        Paginate<Product> products = await _productRepository.GetListAsync(
            index: request.PageRequest.PageIndex,
            size: request.PageRequest.PageSize,
            include: p => p.Include(c => c.Category), // İlişkili veriyi çek
            cancellationToken: cancellationToken
        );

        // 2. Entity listesini DTO listesine çevir
        Paginate<GetListProductListItemDto> mappedProductList = _mapper.Map<Paginate<GetListProductListItemDto>>(products);

        // 3. Sonucu dön
        return mappedProductList;
    }
}
