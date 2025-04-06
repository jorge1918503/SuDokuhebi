namespace SuDokuhebi.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    private async void OnPlayClicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new GamePage());
    }

}