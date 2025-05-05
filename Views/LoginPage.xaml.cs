using SuDokuhebi.Services;

namespace SuDokuhebi.Views;

public partial class LoginPage : ContentPage
{
    private readonly PlayerService _playerService;

    public LoginPage()
    {
        InitializeComponent();
        _playerService = new PlayerService();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Deshabilitar el botón para evitar múltiples clics durante el procesamiento
        var button = sender as Button;
        button.IsEnabled = false;

        string username = usernameEntry.Text;
        string password = passwordEntry.Text;

        // Validar que los campos no estén vacíos
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            messageLabel.TextColor = Colors.Red;
            messageLabel.Text = "Usuario y contraseña son obligatorios";
        }
        else
        {
            // Llamar al servicio de autenticación
            bool isAuthenticated = await _playerService.AuthenticatePlayer(username, password);

            // Mostrar mensaje de éxito o error
            if (isAuthenticated)
            {
                messageLabel.Text = "Inicio de sesión exitoso";
                messageLabel.TextColor = Colors.Green;

                // Redirigir a la pantalla principal
                // Verifica si hay al menos una ventana abierta en la aplicación
                if (Application.Current?.Windows?.Count > 0)
                {
                    // Cambia la pagina principal a la TabbedPage para no poder volver atrás
                    Application.Current.Windows[0].Page = new MenuTabbedPage();
                }
            }
            else
            {
                messageLabel.Text = "Usuario o contraseña incorrectos";
                messageLabel.TextColor = Colors.Red;
                usernameEntry.Text = "";
                passwordEntry.Text = "";
            }
        }

        // Rehabilitar el botón después de que se complete el procesamiento
        button.IsEnabled = true;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NavigationPage(new RegisterPage()));
    }

    // para hacer el loginButton dandole al Intro
    private void OnPasswordEntryCompleted(object sender, EventArgs e)
    {
        OnLoginClicked(loginButton, EventArgs.Empty);
    }
}
