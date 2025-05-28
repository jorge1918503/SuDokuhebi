using SuDokuhebi.Utils;

namespace SuDokuhebi.Views;

public partial class PlayPage : ContentPage
{
    public PlayPage()
    {
        InitializeComponent();

        // Guardar la p�gina actual con tabs
        var currentTabbed = Application.Current?.MainPage;

        // Luego, cuando quieras volver a mostrar las tabs:
        if (currentTabbed != null)
        {
            Application.Current.MainPage = currentTabbed;
        }

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        UsernameLabel.Text = $"Hola, {SessionManager.CurrentUser}";
    }


    private async void OnEasyClicked(object sender, EventArgs e)
    {
        SessionManager.CurrentDifficulty = DifficultyLevel.F�cil;
        // Cambiar a una p�gina sin tabs (por ejemplo, el juego)
        Application.Current.MainPage = new NavigationPage(new GamePage());
    }

    private async void OnPlayClicked(object sender, EventArgs e)
    {
        SessionManager.CurrentDifficulty = DifficultyLevel.Medio;
        // Cambiar a una p�gina sin tabs (por ejemplo, el juego)
        Application.Current.MainPage = new NavigationPage(new GamePage());
    }

    private async void OnHardClicked(object sender, EventArgs e)
    {
        SessionManager.CurrentDifficulty = DifficultyLevel.Dif�cil;
        // Cambiar a una p�gina sin tabs (por ejemplo, el juego)
        Application.Current.MainPage = new NavigationPage(new GameShootPage()); // GamePage alterada para disparar
    }

    private async void OnImposibleClicked(object sender, EventArgs e)
    {
        SessionManager.CurrentDifficulty = DifficultyLevel.Imposible;
        // Cambiar a una p�gina sin tabs (por ejemplo, el juego)
        Application.Current.MainPage = new NavigationPage(new GameImposiblePage());
    }

    private async void OnLogOutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            "�Cerrar sesi�n?",
            "�Est�s seguro?",
            "S�",
            "No");

        if (!confirm)
            return; // El usuario cancel�

        // El usuario confirm�, proceder a cerrar sesi�n
        SessionManager.ClearGame();
        SessionManager.ClearSession();

        await Task.Delay(500);

        Application.Current.MainPage = new NavigationPage(new WelcomePage());
    }

}
