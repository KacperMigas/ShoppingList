using Microsoft.Maui.Controls;
using ShoppingList.Models;
using ShoppingList.Views;

namespace ShoppingList.Views
{
    public partial class CategoryAdd : ContentPage
    {
        public CategoryAdd()
        {
            InitializeComponent();
        }

        private async void OnAddCategoryButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(categoryName.Text))
            {
                CategoryViewModel.Category.Categories.Add(new CategoryModel
                {
                    CategoryName = categoryName.Text
                });

                categoryName.Text = string.Empty;
            }
        }
    }
}
