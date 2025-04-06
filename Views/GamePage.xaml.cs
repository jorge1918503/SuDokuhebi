namespace SuDokuhebi.Views;

public partial class GamePage : ContentPage
{
	public GamePage()
	{
		InitializeComponent();
	}

    private async void OnEndClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}