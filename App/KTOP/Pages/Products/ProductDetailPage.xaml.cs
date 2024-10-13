using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Products;

public partial class ProductDetailPage : ContentPage
{
    private readonly int productId;
	public ProductDetailPage(int productId)
	{
		InitializeComponent();
        this.productId = productId;
	}

    protected async override void OnAppearing()
	{
		base.OnAppearing();
        ProductModel product = await ProductService.GetProductById(productId);
        if (product == null) return;
        else
        {
            CategoryModel category = await CategoryService.GetCategoryById(product.CategoryId);
            HomeModel home = await HomeService.GetHomeById(product.HomeId);
            this.ProdNameL.Text = product.ProductName;
            this.ProdCatL.Text = category.CategoryName;
            this.ProdPurDateL.Text = product.PurchaseDate.ToString("d");
            this.ProdExpDateL.Text = product.ExpiryDate.ToString("d");
            this.ProdOpenDateL.Text = product.OpenDate?.ToString("d") ?? "nie otwarto";
            this.ProdHomeL.Text = home.HomeName;
        }        
	}

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void EditProductBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditProductPage(productId));
    }

    private async void DeleteProductBtn_Clicked_1(object sender, EventArgs e)
    {
        var response = await DisplayAlert("", "Czy napewno chcesz usun¹æ ten produkt?", "Tak", "Nie");
        if (response)
        {
            var result = await ProductService.DeleteProduct(productId);
            if (result)
            {
                await DisplayAlert("", "Produkt zosta³ usuniêty","Ok");
                await Navigation.PopAsync();
            }
            else await DisplayAlert("", "Usuniêcie produktu nie powiod³o siê", "Spróbuj ponownie");
        }
    }
}