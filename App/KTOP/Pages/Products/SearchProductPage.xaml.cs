using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Products;

public partial class SearchProductPage : ContentPage
{
	public SearchProductPage()
	{
		InitializeComponent();
	}

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void ProdSB_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            SearchBar searchBar = (SearchBar)sender;
            if (string.IsNullOrWhiteSpace(searchBar.Text)) CVProdSearch.ItemsSource = null;
            CVProdSearch.ItemsSource = await ProductService.SearchUserProductByName(searchBar.Text);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async void CVProdSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as ProductModel;
        if (currentSelection == null) return;
        await Navigation.PushAsync(new ProductDetailPage(currentSelection.ProductId));
        ((CollectionView)sender).SelectedItem = null;
    }
}