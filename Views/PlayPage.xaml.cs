using SuDokuhebi.Utils;

namespace SuDokuhebi.Views;

public partial class PlayPage : ContentPage
{
    public PlayPage()
    {
        InitializeComponent();

        // Guardar la página actual con tabs
        var currentTabbed = Application.Current?.MainPage;

        // Luego, cuando quieras volver a mostrar las tabs:
        if (currentTabbed != null)
        {
            Application.Current.MainPage = currentTabbed;
        }

    }

    private async void OnEasyClicked(object sender, EventArgs e)
    {
        SessionManager.CurrentDifficulty = DifficultyLevel.Fácil;
        // Cambiar a una página sin tabs (por ejemplo, el juego)
        Application.Current.MainPage = new NavigationPage(new GamePage());
    }

    private async void OnPlayClicked(object sender, EventArgs e)
    {
        SessionManager.CurrentDifficulty = DifficultyLevel.Medio;
        // Cambiar a una página sin tabs (por ejemplo, el juego)
        Application.Current.MainPage = new NavigationPage(new GamePage());
    }

    private async void OnHardClicked(object sender, EventArgs e)
    {
        SessionManager.CurrentDifficulty = DifficultyLevel.Difícil;
        // Cambiar a una página sin tabs (por ejemplo, el juego)
        Application.Current.MainPage = new NavigationPage(new GameShootPage()); // GamePage alterada para disparar
    }

    private async void OnImposibleClicked(object sender, EventArgs e)
    {
        SessionManager.CurrentDifficulty = DifficultyLevel.Imposible;
        // Cambiar a una página sin tabs (por ejemplo, el juego)
        Application.Current.MainPage = new NavigationPage(new GameImposiblePage());
    }
}
