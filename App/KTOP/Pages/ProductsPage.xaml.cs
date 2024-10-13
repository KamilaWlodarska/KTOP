using KTOP.Models;
using KTOP.Services;
using KTOP.Pages.Products;

namespace KTOP.Pages;

public partial class ProductsPage : ContentPage
{
    private readonly List<string> datesList = new() { "Nabycia rosn¹co", "Nabycia malej¹co", "Wa¿noœci rosn¹co", "Wa¿noœci malej¹co", "Otwarcia rosn¹co", "Otwarcia malej¹co" };

    public ProductsPage()
	{
		InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        this.datePicker.ItemsSource = datesList;
        List<HomeModel> homesList = await HomeService.GetAllUserHomes();
        if (homesList != null) this.homePicker.ItemsSource = homesList.ToList();
        List<CategoryModel> categoriesList = await CategoryService.GetAllCategories();
        if (categoriesList != null) this.categoryPicker.ItemsSource = categoriesList.ToList();
        List<ProductModel> productsList = await ProductService.GetAllUserProducts();
        if (productsList != null) this.CVProd.ItemsSource = productsList.ToList();
        this.CVProd.SelectedItem = null;
        this.datePicker.SelectedItem = null;
        this.homePicker.SelectedItem = null;
        this.categoryPicker.SelectedItem = null;
    }

    async void AddProdImgBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddProductPage());
    }

    async void TapProdSearch_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new SearchProductPage());
    }

    private void CVProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as ProductModel;
        if (currentSelection == null) return;
        Navigation.PushAsync(new ProductDetailPage(currentSelection.ProductId));
    }

    private async void datePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedDate = datePicker.SelectedItem;
        if (selectedDate != null)
        {
            List<ProductModel> productsList;
            if (selectedDate.Equals("Nabycia rosn¹co"))
            {
                productsList = await ProductService.GetUserProductsSortedByPurchaseDateAsc();
                if (productsList != null)
                {
                    CVProd.ItemsSource = productsList.ToList();
                    CVProd.SelectedItem = null;
                }
            }
            else if (selectedDate.Equals("Nabycia malej¹co"))
            {
                productsList = await ProductService.GetUserProductsSortedByPurchaseDateDsc();
                if (productsList != null)
                {
                    CVProd.ItemsSource = productsList.ToList();
                    CVProd.SelectedItem = null;
                }
            }
            else if (selectedDate.Equals("Wa¿noœci rosn¹co"))
            {
                productsList = await ProductService.GetUserProductsSortedByExpiryDateAsc();
                if (productsList != null)
                {
                    CVProd.ItemsSource = productsList.ToList();
                    CVProd.SelectedItem = null;
                }
            }
            else if (selectedDate.Equals("Wa¿noœci malej¹co"))
            {
                productsList = await ProductService.GetUserProductsSortedByExpiryDateDsc();
                if (productsList != null)
                {
                    CVProd.ItemsSource = productsList.ToList();
                    CVProd.SelectedItem = null;
                }
            }
            else if (selectedDate.Equals("Otwarcia rosn¹co"))
            {
                productsList = await ProductService.GetUserProductsSortedByOpenDateAsc();
                if (productsList != null)
                {
                    CVProd.ItemsSource = productsList.ToList();
                    CVProd.SelectedItem = null;
                }
            }
            else if (selectedDate.Equals("Otwarcia malej¹co"))
            {
                productsList = await ProductService.GetUserProductsSortedByOpenDateDsc();
                if (productsList != null)
                {
                    CVProd.ItemsSource = productsList.ToList();
                    CVProd.SelectedItem = null;
                }
            }

            this.homePicker.SelectedItem = null;
            this.categoryPicker.SelectedItem = null;
        }
    }

    private async void homePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedHome = homePicker.SelectedItem as HomeModel;
        if (selectedHome != null)
        {
            List<ProductModel> productsList = await ProductService.GetAllUserProductsByHome(selectedHome.HomeId);
            if (productsList != null)
            {
                CVProd.ItemsSource = productsList.ToList();
                CVProd.SelectedItem = null;
                this.datePicker.SelectedItem = null;
                this.categoryPicker.SelectedItem = null;
            }
        }
    }

    private async void categoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedCategory = categoryPicker.SelectedItem as CategoryModel;
        if (selectedCategory != null)
        {
            List<ProductModel> productsList = await ProductService.GetAllUserProductsByCategory(selectedCategory.CategoryId);
            if (productsList != null)
            {
                CVProd.ItemsSource = productsList.ToList();
                CVProd.SelectedItem = null;
                this.datePicker.SelectedItem = null;
                this.homePicker.SelectedItem = null;
            }
        }
    }

    private async void resetBtn_Clicked(object sender, EventArgs e)
    {
        this.datePicker.SelectedItem = null;
        this.homePicker.SelectedItem = null;
        this.categoryPicker.SelectedItem = null;
        List<ProductModel> productsList = await ProductService.GetAllUserProducts();
        if (productsList != null) this.CVProd.ItemsSource = productsList.ToList();
        this.CVProd.SelectedItem = null;
    }
}