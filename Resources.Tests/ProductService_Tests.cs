using Moq;
using Resources.Interfaces;
using Resources.Models;
using Resources.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resources.Tests;

public class ProductService_Tests
{
    private ProductRequest request1;

    private ProductRequest request2;

    private ProductRequest request3;

    private ProductService _productService = new ProductService();

    public ProductService_Tests()
    {

        request1 = new ProductRequest
        {
            ProductName = "test",
            ProductPrice = "12",
            StockCount = "5",
        };

        request2 = new ProductRequest
        {
            ProductName = "test2",
            ProductPrice = "1",
            StockCount = "1",
        };

        request3 = new ProductRequest
        {
            ProductName = "test",
            ProductPrice = "1",
            StockCount = "19",
        };

    }

    // Test Getting Products

    [Fact]
    public void GetOne__Should__ReturnProduct__WhenProductRequestIdMatchesProductInListsId()
    {
        // Arrange

        _productService.AddProductToList(request1);
        var productList = _productService.GetAll();
        request1.ProdudctId = productList.First().ProdudctId;

        // Act

        var product = _productService.GetOne(request1);

        // Assert

        Assert.NotNull(product);
        Assert.Equal(product.ProductName, request1.ProductName);
    }

    [Fact]

    public void GetAll__Should__ReturnListOfProducts()
    {
        // Arrange

        _productService.AddProductToList(request1);
        _productService.AddProductToList(request2);
        List<Product> products = [];
        products.Add(new Product
        {
            ProductName = request1.ProductName,
            ProductPrice = decimal.Parse(request1.ProductPrice),
            StockCount = int.Parse(request1.StockCount),
        });

        products.Add(new Product
        {
            ProductName = request2.ProductName,
            ProductPrice = decimal.Parse(request2.ProductPrice),
            StockCount = int.Parse(request2.StockCount),
        });

        // Act

        var getAllList = _productService.GetAll();

        // Assert

        Assert.True(getAllList.Count() == products.Count());
    }



    // Test Adding Products

    [Fact]
    
    public void AddProductToList__Should__AddProductToList__ReturnTrue()
    {
        //Arrange

        //Act
        var result = _productService.AddProductToList(request1);

        //Assert
        Assert.True(result);
        Assert.True(_productService.GetAll().Count() == 1);
    }

    [Fact]

    public void AddProductToList__ShouldNot__AddProductToListWhenProductRequstNameMatchesExistingProductInListsName__ReturnFalse()
    {

        //Arrange 

        _productService.AddProductToList(request1);

        //Act

        var result = _productService.AddProductToList(request1);

        //Assert

        Assert.False(result);
        Assert.True(_productService.GetAll().Count() == 1);

    }

    // Tests Removing

    [Fact]

    public void RemoveProductFromList__Should__RemoveProductFromListWhenIdMatches__ReturnTrue()
    {
        //Arrange

        _productService.AddProductToList(request1);
        var productList = _productService.GetAll();
        request1.ProdudctId = productList.First().ProdudctId;

        //Act

        var result = _productService.RemoveProductFromList(request1);

        //Assert

        Assert.True(result);
        Assert.True(_productService.GetAll().Count() == 0);
       
    }

    // Tests Updating

    [Fact]

    public void UpdateProduct__Should__UpdateProductValuesWhenIdMatches__ReturnTrue()
    {
        //Arrange

        _productService.AddProductToList(request1);
        var productList = _productService.GetAll();
        request2.ProdudctId = productList.First().ProdudctId;
     

        //Act

        var result = _productService.UpdateProduct(request2);

        //Assert

        Assert.True(result);
        Assert.Equal(_productService.GetOne(request2).ProductName, request2.ProductName);

    }

    [Fact]
    public void UpdateProduct_ShouldNot__UpdateProductValuesWhenProductRequestNameMatchesNameInProductList__ReturnFalse()
    {
        //Arrange

        _productService.AddProductToList(request2);
        var productList = _productService.GetAll();
        _productService.AddProductToList(request1);
        request3.ProdudctId = productList.First().ProdudctId;


        //Act

        var result = _productService.UpdateProduct(request3);

        //Assert

        Assert.False(result);

    }
}
