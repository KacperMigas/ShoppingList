using System.Collections.ObjectModel;

namespace ShoppingList.Models
{
    public class CategoryViewModel
    {
        private static CategoryViewModel categoryView;

        public static CategoryViewModel Category => categoryView ??= new CategoryViewModel();

        public ObservableCollection<CategoryModel> Categories { get; }

        private CategoryViewModel()
        {
            Categories = new ObservableCollection<CategoryModel>();
        }
    }
}
