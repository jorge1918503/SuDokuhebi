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
        // Verifica si hay al menos una ventana abierta en la aplicación
        if (SessionManager.IsLoggedIn)
        {
            // Ir directamente al menú si hay sesión persistida
            // Mostrar una sola vez si la sesión estaba previamente iniciada
            await Task.Delay(300); // Pequeña pausa para que cargue la UI
            await DisplayAlert("Sesión iniciada", $"¡Bienvenido de nuevo, {SessionManager.CurrentUser}!", "OK");

            Application.Current.MainPage = new MenuTabbedPage();
        }
        else
        {
            // Si no hay sesión, ir al login
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}