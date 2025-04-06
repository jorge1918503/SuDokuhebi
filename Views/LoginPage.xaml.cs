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
        // Deshabilitar el bot�n para evitar m�ltiples clics durante el procesamiento
        var button = sender as Button;
        button.IsEnabled = false;

        string username = usernameEntry.Text;
        string password = passwordEntry.Text;

        // Validar que los campos no est�n vac�os
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            messageLabel.TextColor = Colors.Red;
            messageLabel.Text = "Usuario y contrase�a son obligatorios";
        }
        else
        {
            // Llamar al servicio de autenticaci�n
            bool isAuthenticated = await _playerService.AuthenticatePlayer(username, password);

            // Mostrar mensaje de �xito o error
            if (isAuthenticated)
            {
                messageLabel.Text = "Inicio de sesi�n exitoso";
                messageLabel.TextColor = Colors.Green;
                // Redirigir a la pantalla principal si es necesario

            }
            else
            {
                messageLabel.Text = "Usuario o contrase�a incorrectos";
                messageLabel.TextColor = Colors.Red;
                usernameEntry.Text = "";
                passwordEntry.Text = "";
            }
        }

        // Rehabilitar el bot�n despu�s de que se complete el procesamiento
        button.IsEnabled = true;

    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
}