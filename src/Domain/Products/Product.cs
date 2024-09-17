using Domain.Common.Models;

using Domain.Products.ValueObjects;

namespace Domain.Products;

public sealed class Product : AggregateRoot<ProductId>
{
    private Product(): base(ProductId.Create())
    {
    }

    public static Product Create()
    {
        return new Product();
    }
}
