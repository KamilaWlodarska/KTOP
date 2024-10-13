using KTOP.Models;
using KTOP.Services;

namespace KTOP.Pages.Products;

public partial class EditProductPage : ContentPage
{
    private readonly int productId;
    private DateTime? openDate;
    public EditProductPage(int productId)
	{        
		InitializeComponent();
        this.productId = productId;
	}

    public DateTime? OpenDate
    {
        get { return openDate; }
        set
        {
            openDate = value;
            OnPropertyChanged(nameof(OpenDate));
        }
    }

    public bool OpenDateIsSet => openDate.HasValue;

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<CategoryModel> categoriesList = await CategoryService.GetAllCategories();
        if (categoriesList != null) this.categoryPicker.ItemsSource = categoriesList.ToList();
        List<HomeModel> homesList = await HomeService.GetAllUserHomes();
        if (homesList != null) this.homePicker.ItemsSource = homesList.ToList();
        ProductModel product = await ProductService.GetProductById(productId);
        if (product == null) return;
        else
        {
            CategoryModel category = await CategoryService.GetCategoryById(product.CategoryId);
            HomeModel home = await HomeService.GetHomeById(product.HomeId);
            this.EntProductName.Text = product.ProductName;
            this.categoryPicker.Title = category.CategoryName;
            this.categoryPicker.SelectedItem = category;
            this.purchaseDatePicker.Date = product.PurchaseDate;
            this.expiryDatePicker.Date = product.ExpiryDate;
            this.homePicker.Title = home.HomeName;
            this.homePicker.SelectedItem = home;
            try
            {
                openDate = product.OpenDate;
                openDateSwitch.IsToggled = openDate.HasValue;
                openDatePicker.Date = openDate ?? DateTime.Now;
                openDatePicker.IsEnabled = openDate.HasValue;
                openDateSwitchL.Text = "Usuñ datê otwarcia";
                openDateL.TextColor = Color.FromHex("#F6EADD");
                if(openDate == null)
                {
                    openDateSwitchL.Text = "Dodaj datê otwarcia";
                    openDateL.TextColor = Color.FromHex("#737373");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void EditProductBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (EntProductName.Text == null || EntProductName.Text.Length.Equals(0))
            {
                await DisplayAlert("", "Uzupe³nij wszystkie dane", "Ok");
            }
            else
            {
                CategoryModel category = this.categoryPicker.SelectedItem as CategoryModel;
                HomeModel home = this.homePicker.SelectedItem as HomeModel;
                ProductModel editProduct = new()
                {
                    ProductName = this.EntProductName.Text,
                    CategoryId = category.CategoryId,
                    PurchaseDate = purchaseDatePicker.Date,
                    ExpiryDate = expiryDatePicker.Date,
                    HomeId = home.HomeId
                };

                if (openDatePicker.IsEnabled) editProduct.OpenDate = openDatePicker.Date;
                else editProduct.OpenDate = null;

                var result = await ProductService.EditProduct(productId, editProduct);
                if (result)
                {
                    await DisplayAlert("", "Edytowano produkt", "Ok");
                    await Navigation.PopAsync();
                }
                else await DisplayAlert("", "Edycja produktu nie powiod³a siê", "Spróbuj ponownie");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("", "Edycja produktu nie powiod³a siê", "Spróbuj ponownie");
            Console.WriteLine(ex);
        }
    }

    private void openDateSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            if (openDate == null)
            {
                openDate = DateTime.Now;
                OnPropertyChanged(nameof(OpenDate));
            }
            openDatePicker.IsEnabled = true;
            openDateSwitchL.Text = "Usuñ datê otwarcia";
            openDateL.TextColor = Color.FromHex("#F6EADD");
        }
        else
        {
            openDatePicker.IsEnabled = false;
            openDateSwitchL.Text = "Dodaj datê otwarcia";
            openDateL.TextColor = Color.FromHex("#737373");
        }
    }
}