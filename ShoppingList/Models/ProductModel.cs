using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace ShoppingList.Models
{
    public class ProductModel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public string CategoryName { get; set; }

        private bool _isPurchased;
        public bool IsPurchased
        {
            get => _isPurchased;
            set
            {
                if (_isPurchased != value)
                {
                    _isPurchased = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Przeniesiona logika ustawiania właściwości do metody NotifyPropertyChanged()
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
