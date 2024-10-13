using KTOP.Models;
using KTOP.Services;
using KTOP.Pages.Homes;
using KTOP.Pages.Products;

namespace KTOP.Pages;

public partial class StartPage : ContentPage
{
	public StartPage()
	{
		InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<ProductModel> openProductsList = await ProductService.GetAllUserProductsWithOpenDate();
        if(openProductsList != null) this.CVProdOD.ItemsSource = openProductsList.ToList();
        this.CVProdOD.SelectedItem = null;
    }

    async void TapAddProd_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AddProductPage());
    }

    async void TapAddHome_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AddHomePage());
    }

    private void CVProdOD_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as ProductModel;
        if (currentSelection == null) return;
        Navigation.PushAsync(new ProductDetailPage(currentSelection.ProductId));
    }
}