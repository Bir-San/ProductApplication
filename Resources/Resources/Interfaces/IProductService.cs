using Resources.Models;

namespace Resources.Interfaces;

public interface IProductService
{
    bool AddProductToList(ProductRequest productRequest);
    IEnumerable<Product> GetAll();
    Product GetOne(ProductRequest productRequest);
    ProductRequest ProductToRequestConverter(Product product);
    bool RemoveProductFromList(ProductRequest productRequest);
    void Undo();
    bool UpdateProduct(ProductRequest productRequest);
}