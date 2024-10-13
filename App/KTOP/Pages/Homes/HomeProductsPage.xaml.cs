using KTOP.Models;
using KTOP.Services;
using KTOP.Pages.Products;

namespace KTOP.Pages.Homes;

public partial class HomeProductsPage : ContentPage
{
    private readonly int homeId;
	public HomeProductsPage(int homeId)
	{
		InitializeComponent();
        this.homeId = homeId;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<ProductModel> productsList = await ProductService.GetAllUserProductsByHome(homeId);
        if (productsList != null) this.CVHomeProducts.ItemsSource = productsList.ToList();
        this.CVHomeProducts.SelectedItem = null;
    }

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void CVHomeProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as ProductModel;
        if (currentSelection == null) return;
        Navigation.PushAsync(new ProductDetailPage(currentSelection.ProductId));
    }
}