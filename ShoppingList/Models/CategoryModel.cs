using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ShoppingList.Models
{
    public class CategoryModel : INotifyPropertyChanged
    {
        private string _categoryName;

        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
        }

        public ObservableCollection<ProductModel> Products { get; } = new ObservableCollection<ProductModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
