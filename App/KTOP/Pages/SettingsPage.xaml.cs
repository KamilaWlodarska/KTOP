using KTOP.Pages.Settings;

namespace KTOP.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
    }

    async void TapAcc_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AccountPage());
    }

    void TapLang_Tapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushAsync(new LanguagePage());
    }

    void TapCst_Tapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushAsync(new CustomizationPage());
    }

    void TapNtf_Tapped(object sender, TappedEventArgs e)
    {
        //await Navigation.PushAsync(new NotificationPage());
    }

    async void TapInfo_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new AppInfoPage());
    }
}