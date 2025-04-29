namespace SuDokuhebi.Views;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}

	private void OnEnterClicked(object sender, EventArgs e)
    {
        // Redirigir al login
        // Verifica si hay al menos una ventana abierta en la aplicación
        if (Application.Current?.Windows?.Count > 0)
        {
            // Cambia la pagina principal al LoginPage para no poder volver atrás
            Application.Current.Windows[0].Page = new LoginPage();
        }
    }
}