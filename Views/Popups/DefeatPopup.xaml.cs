using CommunityToolkit.Maui.Views;
using Plugin.Maui.Audio;

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
        TimeLabel.Text = $"Tiempo: {time:mm\\:ss}";

    }

    private void OnOkClicked(object sender, EventArgs e)
    {
        Close(true); // Cerrar el popup
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
