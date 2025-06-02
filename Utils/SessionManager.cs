using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace SuDokuhebi.Utils
{
    static class SessionManager
    {
        private const string UserKey = "CurrentUser";
        private const string UserIdKey = "CurrentUserId";
        private const string IsLoggedInKey = "IsLoggedIn";

        // Sesión persistente con Preferences
        public static string CurrentUser
        {
            get => Preferences.Get(UserKey, string.Empty);
            set => Preferences.Set(UserKey, value);
        }

        public static int CurrentUserId
        {
            get => Preferences.Get(UserIdKey, -1);
            set => Preferences.Set(UserIdKey, value);
        }

        public static bool IsLoggedIn
        {
            get => Preferences.Get(IsLoggedInKey, false);
            set => Preferences.Set(IsLoggedInKey, value);
        }

        public static void SetSession(string username, int userId)
        {
            CurrentUser = username;
            CurrentUserId = userId;
            IsLoggedIn = true;
        }

        public static void ClearSession()
        {
            Preferences.Remove(UserKey);
            Preferences.Remove(UserIdKey);
            Preferences.Remove(IsLoggedInKey);
        }

        // Datos de la partida actual
        public static DifficultyLevel? CurrentDifficulty { get; set; } = null;
        public static string CurrentResult { get; set; } = string.Empty;

        public static void ClearGame()
        {
            CurrentDifficulty = null;
            CurrentResult = string.Empty;
        }

    }
}
