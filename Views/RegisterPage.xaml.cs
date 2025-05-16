using SuDokuhebi.Services;

namespace SuDokuhebi.Views;

public partial class RegisterPage : ContentPage
{
    private readonly PlayerService _playerService;

    public RegisterPage()
    {
        InitializeComponent();
        _playerService = new PlayerService();
        UsernameEntry.Text = "";
        PasswordEntry.Text = "";
        Password2Entry.Text = "";
        MessageLabel.Text = "";
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Deshabilitar el botón para evitar múltiples clics durante el procesamiento
        var button = sender as Button;
        button.IsEnabled = false;

        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;
        string password2 = Password2Entry.Text;

        string result = await _playerService.RegisterPlayer(username, password, password2);

        if (result == "Jugador registrado correctamente")
        {
            MessageLabel.TextColor = Colors.Green;
            UsernameEntry.Text = "";
            PasswordEntry.Text = "";
            MessageLabel.Text = "";
            await DisplayAlert("", result, "OK");
            // Redirigir a la pantalla de inicio de sesión
            await Navigation.PopAsync();
        }
        else
        {
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.Text = result;
            UsernameEntry.Text = "";
            PasswordEntry.Text = "";
            Password2Entry.Text = "";
        }

        // Rehabilitar el botón después de que se complete el procesamiento
        button.IsEnabled = true;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        UsernameEntry.Text = "";
        PasswordEntry.Text = "";
        MessageLabel.Text = "";
        await Navigation.PopAsync();
    }

    // para hacer el loginButton dandole al Intro
    private void OnPasswordEntryCompleted(object sender, EventArgs e)
    {
        OnRegisterClicked(RegisterButton, EventArgs.Empty);
    }


    bool isPasswordVisible = false;
    private void OnTogglePasswordVisibilityClicked(object sender, EventArgs e)
    {
        isPasswordVisible = !isPasswordVisible;
        PasswordEntry.IsPassword = !isPasswordVisible;

        togglePasswordVisibilityButton.Source = isPasswordVisible ? "eye_open.png" : "eye_closed.png";
    }


    bool isPassword2Visible = false;
    private void OnTogglePassword2VisibilityClicked(object sender, EventArgs e)
    {
        isPassword2Visible = !isPassword2Visible;
        Password2Entry.IsPassword = !isPassword2Visible;

        togglePassword2VisibilityButton.Source = isPassword2Visible ? "eye_open.png" : "eye_closed.png";
    }


}