using Catalog.Application.Features.Products.Rules;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.Update;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, UpdateProductCommandResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductBusinessRules _productBusinessRules;

    public UpdateProductCommandHandler(IProductRepository productRepository, ProductBusinessRules productBusinessRules)
    {
        _productRepository = productRepository;
        _productBusinessRules = productBusinessRules;
    }

    public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        
        Product product = await _productBusinessRules.ProductShouldExist(request.Id, cancellationToken);

      
        await _productBusinessRules.CategoryShouldExist(request.CategoryId, cancellationToken);

        await _productBusinessRules.ProductNameShouldBeUniqueInCategory(
            request.Name,
            request.CategoryId,
            excludeProductId: request.Id, 
            cancellationToken: cancellationToken
        );

        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;

        
        if (!string.IsNullOrEmpty(request.PictureFileName))
        {
            product.PictureFileName = request.PictureFileName;
        }

        
        await _productRepository.UpdateAsync(product);

        return new UpdateProductCommandResponse(
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.UpdatedDate ?? DateTime.UtcNow
        );
    }
}