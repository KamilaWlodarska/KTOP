using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Products;

public partial class AddProductPage : ContentPage
{
	public AddProductPage()
	{
		InitializeComponent();
	}

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    protected async override void OnAppearing()
	{
		base.OnAppearing();
		List<CategoryModel> categoriesList = await CategoryService.GetAllCategories();
		if(categoriesList != null) this.categoryPicker.ItemsSource = categoriesList.ToList();
		List<HomeModel> homesList = await HomeService.GetAllUserHomes();
		if(homesList != null) this.homePicker.ItemsSource = homesList.ToList();
	}

    private async void AddProductBtn_Clicked(object sender, EventArgs e)
    {
		try
		{
            if (EntProductName.Text == null || EntProductName.Text.Length.Equals(0) || categoryPicker.SelectedItem == null || homePicker.SelectedItem == null)
            {
                await DisplayAlert("", "Uzupe³nij wszystkie dane", "Ok");
            }
            else
            {
                CategoryModel category = (CategoryModel)this.categoryPicker.SelectedItem;
                HomeModel home = (HomeModel)this.homePicker.SelectedItem;
                ProductModel addProduct = new()
                {
                    ProductName = this.EntProductName.Text,
                    CategoryId = category.CategoryId,
                    PurchaseDate = DateTime.Now,
                    ExpiryDate = expiryDatePicker.Date,
                    OpenDate = null,
                    HomeId = home.HomeId
                };

                var result = await ProductService.AddProduct(addProduct);
                if (result)
                {
                    await DisplayAlert("", "Dodano nowy produkt", "Ok");
                    await Navigation.PopAsync();
                }
                else await DisplayAlert("", "Dodawanie produktu nie powiod³o siê", "Spróbuj ponownie");
            }
        }
        catch (Exception)
        {
            await DisplayAlert("", "Dodawanie produktu nie powiod³o siê", "Spróbuj ponownie");
        }
    }
}