using KTOP.Services;
using KTOP.Pages.Products;
using Plugin.Maui.Calendar.Models;

namespace KTOP.Pages;

public partial class CalendarPage : ContentPage
{
    public EventCollection Events { get; set; }
    public CalendarPage()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        cal.ShownDate = DateTime.Now;
        cal.SelectedDate = DateTime.Now;
        Events = new EventCollection();

        var eventDates = await ProductService.GetAllUserProductsExpirationDates();

        foreach (var date in eventDates)
        {
            var products = await ProductService.GetAllUserProductsByExpirationDate(date);
            var eventModels = products.Select(product => new EvenModel { ProductId = product.ProductId, Name = product.ProductName }).ToList();
            Events[date] = eventModels;
        }

        BindingContext = this;
    }

    private async void TapProd_Tapped(object sender, TappedEventArgs e)
    {
        var tappedEvent = (EvenModel)((BindableObject)sender).BindingContext;

        if (tappedEvent != null)
        {
            await Navigation.PushAsync(new ProductDetailPage(tappedEvent.ProductId));
        }
    }
}

internal class EvenModel
{
    public int ProductId { get; set; }
    public string Name { get; set; }
}