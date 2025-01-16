using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class ProductView : ContentView
{
    public ProductView()
    {
        InitializeComponent();
    }

    public ProductView(ProductModel productModel, Action<ProductModel> onIncrement,Action<ProductModel> onDecrement,Action<ProductModel> onPurchased,Action<ProductModel> onDelete)
    {
        InitializeComponent();
        BindingContext = productModel;

        IncrementButton.Clicked += (s, e) => onIncrement?.Invoke(productModel);
        DecrementButton.Clicked += (s, e) => onDecrement?.Invoke(productModel);
        PurchasedButton.Clicked += (s, e) => onPurchased?.Invoke(productModel);
        DeleteButton.Clicked += (s, e) => onDelete?.Invoke(productModel);
    }
}