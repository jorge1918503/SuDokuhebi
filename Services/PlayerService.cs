using SuDokuhebi.Data;
using SuDokuhebi.Utils;
using SuDokuhebi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuDokuhebi.Services
{
    class PlayerService
    {
        private readonly AppDbContext _context;

        public PlayerService()
        {
            _context = new AppDbContext();
        }

        public async Task<string> RegisterPlayer(string username, string password, string password2)
        {
            // Validar que el usuario no exista ya en la base de datos
            var existingPlayer = await _context.Players.FirstOrDefaultAsync(p => p.name == username);
            if (existingPlayer != null)
                return "El jugador ya existe";

            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(password2))
                return "Usuario y contraseña son obligatorios";

            // Validar que las contraseñas coincidan
            if (password != password2)
                return "Las contraseñas no coinciden";

            // Hashear contraseña
            string hashedPassword = PasswordHasher.HashPassword(password);

            var player = new Player
            {
                name = username,
                password = hashedPassword,
                highestScore = 0
            };

            // Agregar el nuevo jugador a la base de datos
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return "Jugador registrado correctamente";
        }

        public async Task<bool> AuthenticatePlayer(string username, string password)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.name == username);
            if (player == null)
                return false;

            bool passwordValid = PasswordHasher.VerifyPassword(password, player.password);
            if (passwordValid)
            {
                // Guardar en Preferences a través de SessionManager
                SessionManager.SetSession(player.name, player.id_player);
            }

            return passwordValid;
        }


        public async Task<Player> GetPlayerByIdAsync(int playerId)
        {
            return await _context.Players.FindAsync(playerId);
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Player>> GetPlayersOrderedByHighestScoreAsync()
        {
            using var context = new AppDbContext();
            return await context.Players
                .OrderByDescending(p => p.highestScore)
                .ToListAsync();
        }
    }
}
