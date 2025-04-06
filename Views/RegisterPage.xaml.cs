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
        MessageLabel.Text = result;
        UsernameEntry.Text = "";
        PasswordEntry.Text = "";

        // Rehabilitar el botón después de que se complete el procesamiento
        button.IsEnabled = true;
    }
}