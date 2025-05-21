using SuDokuhebi.Utils;

namespace SuDokuhebi.Views;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }

    private async void OnEnterClicked(object sender, EventArgs e)
    {
        // Redirigir al login
        // Verifica si hay al menos una ventana abierta en la aplicaci�n
        if (SessionManager.IsLoggedIn)
        {
            // Ir directamente al men� si hay sesi�n persistida
            // Mostrar una sola vez si la sesi�n estaba previamente iniciada
            await Task.Delay(300); // Peque�a pausa para que cargue la UI
            await DisplayAlert("Sesi�n iniciada", $"�Bienvenido de nuevo, {SessionManager.CurrentUser}!", "OK");

            Application.Current.MainPage = new MenuTabbedPage();
        }
        else
        {
            // Si no hay sesi�n, ir al login
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}