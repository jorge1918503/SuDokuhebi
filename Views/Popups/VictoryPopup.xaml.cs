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
        Close(true); // Cerrar el popup
        
    }

    private async void PlayKillSound()
    {
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
        await Task.Delay(700);
        var audioStream = await FileSystem.OpenAppPackageFileAsync("victory.mp3");
        var player = _audioManager.CreatePlayer(audioStream);
        player.Loop = false;
        player.Play();
    }

}
