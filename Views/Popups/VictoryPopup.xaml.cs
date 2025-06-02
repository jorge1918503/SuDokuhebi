using CommunityToolkit.Maui.Views;
using Plugin.Maui.Audio;
using SuDokuhebi.Utils;
using System.Threading.Tasks;

namespace SuDokuhebi.Views.Popups;

public partial class VictoryPopup : Popup
{
    private readonly IAudioManager _audioManager;

    public VictoryPopup(int movements, int timeElapsed, bool _soundEnabled)
    {
        InitializeComponent();

        _audioManager = AudioManager.Current;
        if (_soundEnabled)
        {
            PlayKillSound();
            PlayVictorySound();
        }

        MovementsLabel.Text = $"Movimientos: {movements}";

        var time = TimeSpan.FromSeconds(timeElapsed);
        TimeLabel.Text = $"Tiempo: {time:mm\\:ss}";

    }

    private void OnOkClicked(object sender, EventArgs e)
    {
        Close("menu"); // Cerrar el popup y vuelve al menú principal

    }

    private async void OnAgainClicked(object sender, EventArgs e)
    {
        // Al clicar "Jugar de nuevo", se reinicia la partida según la dificultad actual
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


    private async void PlayKillSound()
    {
        // Reproduce un sonido diferente según la dificultad actual
        if (SessionManager.CurrentDifficulty == DifficultyLevel.Difícil || SessionManager.CurrentDifficulty == DifficultyLevel.Imposible)
        {
            var audioStream = await FileSystem.OpenAppPackageFileAsync("tirachinas.mp3");
            var player = _audioManager.CreatePlayer(audioStream);
            player.Loop = false;
            player.Play();
        }
        else
        {
            var audioStream = await FileSystem.OpenAppPackageFileAsync("blade.mp3");
            var player = _audioManager.CreatePlayer(audioStream);
            player.Loop = false;
            player.Play();
        }
    }

    private async void PlayVictorySound()
    {
        // Reproduce el sonido de victoria 
        await Task.Delay(700);
        var audioStream = await FileSystem.OpenAppPackageFileAsync("victory.mp3");
        var player = _audioManager.CreatePlayer(audioStream);
        player.Loop = false;
        player.Play();
    }

}
