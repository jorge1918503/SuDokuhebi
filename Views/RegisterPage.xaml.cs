using SuDokuhebi.Services;

namespace SuDokuhebi.Views;

public partial class RegisterPage : ContentPage
{
    private readonly PlayerService _playerService;

    public RegisterPage()
    {
        InitializeComponent();
        _playerService = new PlayerService();
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Deshabilitar el botón para evitar múltiples clics durante el procesamiento
        var button = sender as Button;
        button.IsEnabled = false;

        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        string result = await _playerService.RegisterPlayer(username, password);

        if (result == "Jugador registrado correctamente")
        {
            MessageLabel.TextColor = Colors.Green;
            MessageLabel.Text = result;
            UsernameEntry.Text = "";
            PasswordEntry.Text = "";
            Task.Delay(3000).Wait(); // Esperar 1 segundo para mostrar el mensaje
            // Redirigir a la pantalla de inicio de sesión
            await Navigation.PopAsync();
        }
        else
        {
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = result;
            UsernameEntry.Text = "";
            PasswordEntry.Text = "";
        }

        // Rehabilitar el botón después de que se complete el procesamiento
        button.IsEnabled = true;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        UsernameEntry.Text = "";
        PasswordEntry.Text = "";
        await Navigation.PopAsync();
    }

    // para hacer el loginButton dandole al Intro
    private void OnPasswordEntryCompleted(object sender, EventArgs e)
    {
        OnRegisterClicked(RegisterButton, EventArgs.Empty);
    }

}