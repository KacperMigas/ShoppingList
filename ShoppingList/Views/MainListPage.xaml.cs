using System.Text.Json;
using CommunityToolkit.Maui.Storage;
using ShoppingList.Models;

namespace ShoppingList.Views
{
    public partial class MainListPage : ContentPage
    {
        private static readonly FilePickerFileType JsonFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".json" } },
        });

        private readonly string _filePath = Path.Combine(FileSystem.AppDataDirectory, "products.json");

        public MainListPage()
        {
            InitializeComponent();
            BindingContext = ProductRepository.SharedProduct;
            LoadProducts();
        }

        private void onAddProductClicked(object sender, EventArgs e)
        {
            var name = productTitle.Text;
            var unit = productUnit.Text;
            var quantityText = productQuantity.Text;
            var categoryName = productCategory.Text;

            if (HasInvalidInput(name, unit, quantityText, categoryName, out int quantity))
                return;

            AddNewProduct(name, unit, quantity, categoryName);

            ClearInputFields();
            SaveProducts();
            RefreshProductsView();
        }

        private bool HasInvalidInput(string name, string unit, string quantityText, string categoryName, out int quantity)
        {
            quantity = 0;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(unit) || string.IsNullOrWhiteSpace(quantityText) || string.IsNullOrWhiteSpace(categoryName))
            {
                DisplayAlert("Błąd", "Wszystkie pola muszą być wypełnione.", "OK");
                return true;
            }

            if (!int.TryParse(quantityText, out quantity))
            {
                DisplayAlert("Błąd", "Ilość musi być liczbą całkowitą.", "OK");
                return true;
            }

            if (quantity <= 0)
            {
                DisplayAlert("Błąd", "Ilość nie może być mniejsza lub równa 0.", "OK");
                return true;
            }

            if (IsInputNumber(name, unit, categoryName))
            {
                DisplayAlert("Błąd", "Nazwa, jednostka i kategoria nie mogą być liczbami.", "OK");
                return true;
            }

            return false;
        }

        private bool IsInputNumber(string name, string unit, string categoryName)
        {
            return int.TryParse(name, out _) || int.TryParse(unit, out _) || int.TryParse(categoryName, out _);
        }

        private void AddNewProduct(string name, string unit, int quantity, string categoryName)
        {
            if (BindingContext is Product product)
            {
                var category = CategoryViewModel.Category.Categories
                    .FirstOrDefault(c => c.CategoryName.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

                if (category == null)
                {
                    category = new CategoryModel { CategoryName = categoryName };
                    CategoryViewModel.Category.Categories.Add(category);
                }

                var newProduct = new ProductModel
                {
                    Name = name,
                    Unit = unit,
                    Quantity = quantity,
                    CategoryName = category.CategoryName
                };

                category.Products.Add(newProduct);
                product.Products.Add(newProduct);
            }
        }

        private void ClearInputFields()
        {
            productTitle.Text = string.Empty;
            productUnit.Text = string.Empty;
            productQuantity.Text = string.Empty;
            productCategory.Text = string.Empty;
        }

        private void SaveProducts()
        {
            try
            {
                if (BindingContext is Product product)
                {
                    var json = JsonSerializer.Serialize(product.Products);
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
                        var json = File.ReadAllText(_filePath);
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

                RefreshProductsView();
            }
        }

        private void RefreshProductsView()
        {
            if (BindingContext is Product product)
            {
                ProductsContainer.Children.Clear();

                var sortedProducts = product.Products
                    .OrderBy(p => p.IsPurchased)
                    .ThenBy(p => p.Name);

                foreach (var productModel in sortedProducts)
                {
                    var productView = new ProductView(productModel, OnIncrement_Clicked, OnDecrement_Clicked, OnPurchased_Clicked, OnDelete_Clicked);
                    ProductsContainer.Children.Add(productView);
                }
            }
        }

        private void OnIncrement_Clicked(ProductModel model)
        {
            model.Quantity++;
            SaveProducts();
        }

        private void OnDecrement_Clicked(ProductModel model)
        {
            model.Quantity = Math.Max(model.Quantity - 1, 0);
            SaveProducts();
        }

        private void OnDelete_Clicked(ProductModel productModel)
        {
            if (BindingContext is Product product)
            {
                product.Products.Remove(productModel);
                SaveProducts();
                RefreshProductsView();
            }
        }

        private void OnPurchased_Clicked(ProductModel productModel)
        {
            if (BindingContext is Product product)
            {
                int oldIndex = product.Products.IndexOf(productModel);

                if (oldIndex == -1) return;

                product.Products.RemoveAt(oldIndex);
                productModel.IsPurchased = !productModel.IsPurchased;
                product.Products.Add(productModel);

                SaveProducts();
                RefreshProductsView();
            }
        }
    }
}
