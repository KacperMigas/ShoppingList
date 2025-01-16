using ShoppingList.Models;

namespace ShoppingList.Views
{
    public partial class CategoryView : ContentPage
    {
        public CategoryView()
        {
            InitializeComponent();
            BindingContext = CategoryViewModel.Category; 
        }
    }
}
