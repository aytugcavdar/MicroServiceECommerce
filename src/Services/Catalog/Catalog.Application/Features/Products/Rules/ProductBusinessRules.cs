using BuildingBlocks.CrossCutting.Exceptions.types;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Features.Products.Rules;

public class ProductBusinessRules
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductBusinessRules(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task ProductNameShouldBeUniqueInCategory(
        string name,
        Guid categoryId,
        Guid? excludeProductId = null,
        CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productRepository.GetAsync(
            p => p.Name.ToLower() == name.ToLower() &&
            p.CategoryId == categoryId &&
            (excludeProductId == null || p.Id != excludeProductId),
            cancellationToken: cancellationToken
            );

        if (existingProduct != null)
            throw new BusinessException(
                $"Product with name '{name}' already exists in this category"
            );
    }
    public async Task CategoryShouldExist(
        Guid categoryId,
        CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetAsync(
            c => c.Id == categoryId,
            cancellationToken: cancellationToken
        );

        if (category == null)
            throw new NotFoundException("Category", categoryId);
    }

    /// <summary>
    /// Ürün mevcut olmalı
    /// </summary>
    public async Task<Product> ProductShouldExist(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetAsync(
            p => p.Id == productId,
            include: q => q.Include(p => p.Category),
            cancellationToken: cancellationToken
        );

        if (product == null)
            throw new NotFoundException("Product", productId);

        return product;
    }

    /// <summary>
    /// Stok güncelleme için yeterli stok olmalı
    /// </summary>
    public void StockShouldBeSufficient(int currentStock, int requestedQuantity)
    {
        if (currentStock < requestedQuantity)
            throw new BusinessException(
                $"Insufficient stock. Available: {currentStock}, Requested: {requestedQuantity}"
            );
    }

    
}
