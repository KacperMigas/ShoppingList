using System.Collections.Generic;
using System.Text.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;
public partial class SortedProducts : ContentPage
{
    private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "products.json");

    public SortedProducts()
	{
        BindingContext = ProductRepository.SharedProduct;
        InitializeComponent();
        LoadProducts();
    }

    private void saveProducts()
    {
        try
        {
            if (BindingContext is Product product)
            {
                string json = JsonSerializer.Serialize(product.Products.ToList());
                File.WriteAllText(_filePath, json);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Błąd", $"Nie udało się zapisać produktów: {ex.Message}", "OK");
        }
    }

    private void LoadProducts()
    {
        if (BindingContext is Product product)
        {
            product.Products.Clear();

            if (File.Exists(_filePath))
            {
                try
                {
                    string json = File.ReadAllText(_filePath);
                    var products = JsonSerializer.Deserialize<List<ProductModel>>(json);

                    if (products != null)
                    {
                        foreach (var loadedProduct in products)
                        {
                            product.Products.Add(loadedProduct);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Błąd", $"Nie udało się załadować produktów: {ex.Message}", "OK");
                }
            }
        }
    }

    private void OnIncrement_Clicked(ProductModel model)
    {
        model.Quantity += 1;
        saveProducts();
    }
    private void OnDecrement_Clicked(ProductModel model)
    {
        model.Quantity -= 1;
        if (model.Quantity < 0)
        {
            model.Quantity = 0;
        }
        saveProducts();
    }
    private void OnDelete_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ProductModel productModel)
        {
            if (BindingContext is Product product)
            {
                product.Products.Remove(productModel);
                saveProducts();
            }
        }
    }
    
    private void OnPurchased_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is ProductModel productModel)
        {
            if (BindingContext is Product product)
            {
                int oldIndex = product.Products.IndexOf(productModel);

                if (oldIndex == -1) return;

                product.Products.RemoveAt(oldIndex);

                productModel.IsPurchased = !productModel.IsPurchased;

                product.Products.Add(productModel);

                saveProducts();
            }
        }
    }
}