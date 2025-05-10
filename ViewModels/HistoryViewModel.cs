using SuDokuhebi.Services;
using SuDokuhebi.Models;
using SuDokuhebi.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuDokuhebi.ViewModels
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Game> Games { get; set; } = new();

        public string PlayerName => SessionManager.CurrentUser ?? "UknownUser";

        public async Task LoadHistoryAsync()
        {
            var gameService = new GameService();
            var games = await gameService.GetGamesByPlayerIdAsync(SessionManager.CurrentUserId);

            Games.Clear();
            foreach (var game in games)
            {
                Games.Add(game);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
