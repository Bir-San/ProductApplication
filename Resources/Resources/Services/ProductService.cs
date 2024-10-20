
using Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;
using System.Buffers.Text;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Resources.Interfaces;

namespace Resources.Services;

public class ProductService : IProductService
{
    private List<Product> _products = [];
    private Stack<UndoRequest> _undoRequestList = [];
    private readonly FileService _fileService;
    private readonly string _directory = Path.Combine(Directory.GetCurrentDirectory(), "Product_List");

    private enum Actions
    {
        Created = 0,
        Removed = 1,
        Updated = 2,
    }

    public ProductService()
    {
        _fileService = new FileService(_directory);
    }
    public bool AddProductToList(ProductRequest productRequest)
    {
        try
        {
            if (_products.Any(x => x.ProductName == productRequest.ProductName))
            {
                return false;
            }


            var product = RequestToProductConverter(productRequest);



            if (!productRequest.IsUndo)
            {
                var undoRequest = new UndoRequest
                {
                    RequestedProduct = ProductToRequestConverter(product),
                    Action = (int)Actions.Created,
                };
                undoRequest.RequestedProduct.IsUndo = true;
                _undoRequestList.Push(undoRequest);
            }

            _products.Add(product);

            return _fileService.SaveToFile(JsonConvert.SerializeObject(_products));
        }

        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }



    public IEnumerable<Product> GetAll()
    {
        _products.Clear();

        var json = _fileService.GetFromFile();
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                _products = JsonConvert.DeserializeObject<List<Product>>(json)!;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        return _products as IEnumerable<Product>;
    }

    public Product GetOne(ProductRequest productRequest)
    {
        try
        {
            return _products.FirstOrDefault(x => x.ProdudctId == productRequest.ProdudctId)!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        return null!;
    }

    public bool RemoveProductFromList(ProductRequest productRequest)
    {
        var removeProduct = _products.FirstOrDefault(x => x.ProdudctId == productRequest.ProdudctId)!;
        if (removeProduct != null)
        {
            try
            {
                if (!productRequest.IsUndo)
                {
                    var undoRequest = new UndoRequest
                    {
                        RequestedProduct = ProductToRequestConverter(removeProduct),
                        Action = (int)Actions.Removed,
                    };

                    undoRequest.RequestedProduct.IsUndo = true;
                    _undoRequestList.Push(undoRequest);

                }

                _products.Remove(removeProduct);

                _fileService.SaveToFile(JsonConvert.SerializeObject(_products) ?? "");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
        return false;
    }

    public bool UpdateProduct(ProductRequest productRequest)
    {
        var originalProduct = _products.FirstOrDefault(x => x.ProdudctId == productRequest.ProdudctId)!;

        if (_products.Any(x => x.ProductName == productRequest.ProductName && x.ProdudctId != originalProduct.ProdudctId))
        {
            return false;
        }

        if (originalProduct != null)
        {
            try
            {
                if (!productRequest.IsUndo)
                {
                    var undoRequest = new UndoRequest
                    {
                        RequestedProduct = ProductToRequestConverter(originalProduct),
                        Action = (int)Actions.Updated,
                    };
                    undoRequest.RequestedProduct.IsUndo = true;

                    _undoRequestList.Push(undoRequest);

                }
                var tempProduct = RequestToProductConverter(productRequest);
                if (tempProduct != null)
                {
                    originalProduct.ProductName = tempProduct.ProductName;
                    originalProduct.ProductPrice = tempProduct.ProductPrice;
                    originalProduct.StockCount = tempProduct.StockCount;

                }
                var json = JsonConvert.SerializeObject(_products);
                _fileService.SaveToFile(json);
                return true;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        return false;
    }

    public void Undo()
    {
        if (_undoRequestList != null)
        {
            try
            {
                _undoRequestList.Peek().RequestedProduct.IsUndo = true;
                switch (_undoRequestList.Peek().Action)
                {
                    case ((int)Actions.Created):
                        RemoveProductFromList(_undoRequestList.Pop().RequestedProduct);
                        break;
                    case ((int)Actions.Updated):
                        UpdateProduct(_undoRequestList.Pop().RequestedProduct);
                        break;
                    case ((int)Actions.Removed):
                        AddProductToList(_undoRequestList.Pop().RequestedProduct);
                        break;
                    default:
                        break;

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }
    }

    public ProductRequest ProductToRequestConverter(Product product)
    {
        try
        {
            ProductRequest productRequest = new ProductRequest
            {
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice.ToString(),
                StockCount = product.StockCount.ToString(),
                ProdudctId = product.ProdudctId,
            };
            return productRequest;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null!;
        }

    }

    public Product RequestToProductConverter(ProductRequest productRequest)
    {
        if (productRequest == null || productRequest.ProductName == null)
            return null!;

        if ((!int.TryParse(productRequest.StockCount, out int stock) && stock >= 0) || (!decimal.TryParse(productRequest.ProductPrice, out decimal price) && price > 0))
            return null!;
        try
        {
            Product product = new Product
            {
                ProductName = productRequest.ProductName,
                ProductPrice = price,
                StockCount = stock,
                ProdudctId = Guid.NewGuid().ToString(),
            };

            return product;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null!;
        }
    }
}