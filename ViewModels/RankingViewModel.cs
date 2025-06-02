using SuDokuhebi.Services;
using SuDokuhebi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SuDokuhebi.ViewModels
{
    public class RankingViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PlayerDisplay> DisplayPlayers { get; set; } = new();

        public async Task LoadRankingAsync()
        {
            // Cargar los jugadores ordenados por su puntuación más alta
            var playerService = new PlayerService();
            var players = await playerService.GetPlayersOrderedByHighestScoreAsync();

            DisplayPlayers.Clear();
            // Mostrar medallas a los primeros 3 jugadores
            for (int i = 0; i < players.Count; i++)
            {
                var medal = i switch
                {
                    0 => "gold.png",
                    1 => "silver.png",
                    2 => "bronze.png",
                    _ => null
                };

                DisplayPlayers.Add(new PlayerDisplay
                {
                    Name = players[i].name,
                    HighestScore = players[i].highestScore,
                    MedalImage = medal
                });
            }
        }

        // Implementación de INotifyPropertyChanged para notificar cambios en las propiedades
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
