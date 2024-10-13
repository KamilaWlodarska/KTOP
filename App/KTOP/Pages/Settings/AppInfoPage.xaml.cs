namespace KTOP.Pages.Settings;

public partial class AppInfoPage : ContentPage
{
	public AppInfoPage()
	{
		InitializeComponent();
    }

    void BackImgBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}