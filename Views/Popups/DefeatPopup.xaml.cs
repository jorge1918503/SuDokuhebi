using CommunityToolkit.Maui.Views;
using Plugin.Maui.Audio;
using SuDokuhebi.Utils;

namespace SuDokuhebi.Views.Popups;

public partial class DefeatPopup : Popup
{
    private readonly IAudioManager _audioManager;

    public DefeatPopup(int movements, int timeElapsed, bool _soundEnabled)
    {
        InitializeComponent();

        _audioManager = AudioManager.Current;
        if (_soundEnabled)
        {
            PlayDefeatSound();
        }


        MovementsLabel.Text = $"Movimientos: {movements}";
        var time = TimeSpan.FromSeconds(timeElapsed);

        if (SessionManager.CurrentDifficulty == DifficultyLevel.Imposible)
        {
            time = TimeSpan.FromSeconds(60 - timeElapsed);
        }
        else
        {
            time = TimeSpan.FromSeconds(timeElapsed);
        }

        TimeLabel.Text = $"Tiempo: {time:mm\\:ss}";

    }

    private void OnOkClicked(object sender, EventArgs e)
    {
        Close("menu"); // Cerrar el popup
    }

    private async void OnAgainClicked(object sender, EventArgs e)
    {
        switch (SessionManager.CurrentDifficulty)
        {
            case DifficultyLevel.Fácil:
                SessionManager.CurrentDifficulty = DifficultyLevel.Fácil;
                Application.Current.MainPage = new NavigationPage(new GamePage());
                Close("again"); // Cerrar el popup
                break;

            case DifficultyLevel.Medio:
                SessionManager.CurrentDifficulty = DifficultyLevel.Medio;
                Application.Current.MainPage = new NavigationPage(new GamePage());
                Close("again"); // Cerrar el popup
                break;

            case DifficultyLevel.Difícil:
                SessionManager.CurrentDifficulty = DifficultyLevel.Difícil;
                Application.Current.MainPage = new NavigationPage(new GameShootPage());
                Close("again"); // Cerrar el popup
                break;

            case DifficultyLevel.Imposible:
                SessionManager.CurrentDifficulty = DifficultyLevel.Imposible;
                Application.Current.MainPage = new NavigationPage(new GameImposiblePage());
                Close("again"); // Cerrar el popup
                break;

            default:
                // En caso de valor inesperado, puedes navegar a una página por defecto o lanzar una excepción.
                await Application.Current.MainPage.DisplayAlert("Error", "Dificultad desconocida", "OK");
                break;
        }
    }

    private async void PlayDefeatSound()
    {
        // Fix: Use the correct method from IAudioManager to create a player
        var audioStream = await FileSystem.OpenAppPackageFileAsync("snakebite.mp3");
        var player = _audioManager.CreatePlayer(audioStream);
        player.Loop = false;
        player.Play();
    }
}
