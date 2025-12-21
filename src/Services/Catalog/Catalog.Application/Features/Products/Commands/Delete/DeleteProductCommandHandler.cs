using Catalog.Application.Features.Products.Rules;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Features.Products.Commands.Delete;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, DeleteProductCommandResponse>
{
    private readonly IProductRepository _productRepository;
    private readonly ProductBusinessRules _productBusinessRules;

    public DeleteProductCommandHandler(IProductRepository productRepository, ProductBusinessRules productBusinessRules)
    {
        _productRepository = productRepository;
        _productBusinessRules = productBusinessRules;
    }

    public async Task<DeleteProductCommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
       
        Product product = await _productBusinessRules.ProductShouldExist(request.Id, cancellationToken);

        
        await _productRepository.DeleteAsync(product);

        await _productRepository.SaveChangesAsync(cancellationToken);

        return new DeleteProductCommandResponse(product.Id);
    }
}