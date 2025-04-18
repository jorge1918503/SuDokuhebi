namespace SuDokuhebi.Views;

public partial class PlayPage : ContentPage
{
	public PlayPage()
	{
		InitializeComponent();

        // Guardar la página actual con tabs
        var currentTabbed = Application.Current.MainPage;

        // Luego, cuando quieras volver a mostrar las tabs:
        Application.Current.MainPage = currentTabbed;
    }

    private async void OnPlayClicked(object sender, EventArgs e)
    {
        // Cambiar a una página sin tabs (por ejemplo, el juego)
        Application.Current.MainPage = new NavigationPage(new GamePage());
    }

}