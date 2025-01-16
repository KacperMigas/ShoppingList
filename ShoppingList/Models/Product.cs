using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ShoppingList.Models
{
    public class Product : INotifyPropertyChanged
    {
        private ObservableCollection<ProductModel> _products = new ObservableCollection<ProductModel>();

        public ObservableCollection<ProductModel> Products
        {
            get => _products;
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        public Product()
        {
            _products.CollectionChanged += HandleProductsCollectionChanged;
        }

        private void HandleProductsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FilteredProducts));
        }

        public IEnumerable<ProductModel> FilteredProducts => _products.Where(product => !product.IsPurchased);

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class ProductRepository
    {
        private static readonly Product _sharedProduct = new Product();

        public static Product SharedProduct => _sharedProduct;
    }
}
