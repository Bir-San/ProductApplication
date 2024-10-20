using Resources;
using Resources.Models;
using Resources.Services;

string filePath = @"C:\Users";


Product testProduct = new Product { InStock = true, ProductName = "Champagne", ProductPrice = 188, ProdudctId = Guid.NewGuid().ToString(), StockCount = 45 };

FileService testFileService = new FileService(filePath);
ProductService testProductService = new ProductService();



