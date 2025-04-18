using CommunityToolkit.Maui.Views;
using Plugin.Maui.Audio;
using System.Threading.Tasks;

namespace SuDokuhebi.Views.Popups;

public partial class VictoryPopup : Popup
{
    private readonly IAudioManager _audioManager;

    public VictoryPopup(int movements, int timeElapsed)
    {
        InitializeComponent();

        _audioManager = AudioManager.Current;
        PlayVictorySound();

        MovementsLabel.Text = $"Movimientos: {movements}";

        var time = TimeSpan.FromSeconds(timeElapsed);
        TimeLabel.Text = $"Tiempo: {time:mm\\:ss}";

    }

    private void OnOkClicked(object sender, EventArgs e)
    {
        Close(true); // Cerrar el popup
        
    }

    private async void PlayVictorySound()
    {
        // Fix: Use the correct method from IAudioManager to create a player
        var audioStream = await FileSystem.OpenAppPackageFileAsync("victory.mp3");
        var player = _audioManager.CreatePlayer(audioStream);
        player.Loop = false;
        player.Play();
    }
}
