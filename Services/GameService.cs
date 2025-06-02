using Microsoft.EntityFrameworkCore;
using SuDokuhebi.Data;
using SuDokuhebi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuDokuhebi.Services
{
    class GameService
    {
        private readonly AppDbContext _context;

        public GameService()
        {
            _context = new AppDbContext();
        }

        public async Task<bool> SaveGame(int playerId, string difficulty, string result, TimeSpan time, int movements, int score)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(difficulty) || string.IsNullOrWhiteSpace(result))
                return false;
            var game = new Game
            {
                id_player = playerId,
                difficulty = difficulty,
                result = result,
                time = time,
                movements = movements,
                score = score
            };
            // Agregar el nuevo juego a la base de datos
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<Game>> GetGamesByPlayerIdAsync(int playerId)
        {
            // Obtener los juegos del jugador por su ID 
            using var context = new AppDbContext();
            return await context.Games
                .Where(g => g.id_player == playerId)
                .OrderByDescending(g => g.score)
                .ToListAsync();
        }

    }
}
