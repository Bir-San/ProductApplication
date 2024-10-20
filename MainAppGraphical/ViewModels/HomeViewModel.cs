using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Resources.Models;
using Resources.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Resources.Interfaces;

namespace MainAppGraphical.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IProductService _productService;
    private bool _updateProduct;
    private bool _validName;
    private bool _validPrice;
    private bool _validStock;
    private bool _noDuplicateName;


    [ObservableProperty]
    private ObservableCollection<Product> _products = [];

    [ObservableProperty]
    private ProductRequest _selectedProduct = new();

    [ObservableProperty]
    private string _nameMsg = "";

    [ObservableProperty]
    private string _priceMsg = "";

    [ObservableProperty]
    private string _stockMsg = "";

    [ObservableProperty]
    private string _saveText = "Save Product";

    public HomeViewModel(IServiceProvider serviceProvider, ProductService productService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        UpdateList();
    }

    [RelayCommand]
    public void SaveProduct ()
    {

        NameValidate();
        PriceValidate();
        StockValidate();
        DuplicateCheck();

        if (!_noDuplicateName)
        {
            NameMsg = "Name Already exists";
            return;
        } 

        if (_validName && _validPrice && _validStock)
        {
            
            

            if (!_updateProduct)
            {
                _productService.AddProductToList(SelectedProduct);

            }
            else
            {
                _productService.UpdateProduct(SelectedProduct);
            }
            UpdateList();
        }
        

        
    }

    [RelayCommand]
    public void Remove (Product product)
    {
        _productService.RemoveProductFromList(_productService.ProductToRequestConverter(product));
        UpdateList();
    }

    [RelayCommand]
    public void Cancel()
    {
        SelectedProduct = new();
        SaveText = "Save Product";
        NameMsg = "";
        PriceMsg = "";
        StockMsg = "";
        UpdateList();
    }

    [RelayCommand]
    public void Update (Product product)
    {
        if(product != null)
        {
            SelectedProduct = _productService.ProductToRequestConverter(product);
            _updateProduct = true;
            SaveText = "Update Product";
        }
        
    }

    public void UpdateList ()
    {
        Products.Clear();

        var tempList = _productService.GetAll();

        if(tempList != null)
        {
            foreach (Product product in tempList)
            { 
                Products.Add(product);
            }
        }

        SaveText = "Save Product";
        SelectedProduct = new();
        _updateProduct = false;
    }

    public void NameValidate()
    {
        if(string.IsNullOrWhiteSpace(SelectedProduct.ProductName))
        {
            _validName = false;
            NameMsg = "Please fill in a name";
            return;
        }
        
        _validName = true;
        NameMsg = "";
    }

    public void PriceValidate()
    {
        if(string.IsNullOrWhiteSpace(SelectedProduct.ProductPrice))
        {
            _validPrice = false;
            PriceMsg = "Please enter a price";
            return;
        }

        if(!decimal.TryParse(SelectedProduct.ProductPrice, out decimal tempPrice))
        {
            _validPrice = false;
            PriceMsg = "Please enter a price in numbers";
            return;
        }

        if(tempPrice < (decimal)0)
        {
            _validPrice = false;
            PriceMsg = "Please enter a positive price value";
            return;
        }

        _validPrice = true;
        PriceMsg = "";

    }

    public void StockValidate()
    {
        if (string.IsNullOrWhiteSpace(SelectedProduct.StockCount))
        {
            _validStock = false;
            StockMsg = "Please enter a stock amount";
            return;
        }

        if (!int.TryParse(SelectedProduct.StockCount.Trim(), out int tempStock))
        {
            _validStock = false;
            StockMsg = "Please enter a stock in whole numbers";
            return;
        }

        if (tempStock < 0)
        {
            _validStock = false;
            StockMsg = "Please enter a positive or zero stock amount";
            return;
        }

        _validStock = true;
        StockMsg = "";

    }

    public void DuplicateCheck()
    {
        if(_updateProduct)
        {
            if (Products.Any(x => x.ProductName == SelectedProduct.ProductName && x.ProdudctId != SelectedProduct.ProdudctId))
            {
                _noDuplicateName = false;
                return;
            }
        }
        else if(Products.Any(x => x.ProductName == SelectedProduct.ProductName))
        {
            _noDuplicateName = false;
            return;
        }

        _noDuplicateName = true;

    }

    [RelayCommand]
    public void Undo()
    {
        _productService.Undo();
        UpdateList();
    }

}