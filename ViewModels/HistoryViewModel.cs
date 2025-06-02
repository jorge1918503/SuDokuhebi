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

        // Esta lista mantiene todas las partidas cargadas inicialmente
        private List<Game> AllGames { get; set; } = new();
        // Nombre del jugador actual
        public string PlayerName => SessionManager.CurrentUser ?? "UnknownUser";

        public async Task LoadHistoryAsync()
        {
            // Cargar el historial de partidas del jugador actual
            var gameService = new GameService();
            var games = await gameService.GetGamesByPlayerIdAsync(SessionManager.CurrentUserId);

            AllGames = games.ToList(); // Guardamos la lista completa original

            Games.Clear();
            foreach (var game in AllGames)
            {
                Games.Add(game);
            }
        }

        public void ApplyFilter(string field)
        {
            IEnumerable<Game> filtered = AllGames;

            switch (field)
            {
                // Filtrar por dificultad (ponemos nosotros el orden)
                case "difficulty":
                    var difficultyOrder = new List<string> { "Imposible", "Difícil", "Medio", "Fácil" };
                    filtered = AllGames.OrderBy(g =>
                    {
                        var index = difficultyOrder.IndexOf(g.difficulty);
                        return index >= 0 ? index : int.MaxValue;
                    });
                    break;
                // Filtrar por resultado
                case "result":
                    filtered = AllGames.OrderByDescending(g => g.result);
                    break;
                // Filtrar por puntuación
                case "score":
                    filtered = AllGames.OrderByDescending(g => g.score);
                    break;
                // Filtrar por tiempo
                case "time":
                    filtered = AllGames.OrderBy(g => g.time);
                    break;
                // Filtrar por movimientos
                case "movements":
                    filtered = AllGames.OrderBy(g => g.movements);
                    break;
            }

            Games.Clear();
            foreach (var game in filtered)
                Games.Add(game);
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }


}
