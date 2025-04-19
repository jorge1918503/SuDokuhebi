using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuDokuhebi.Utils
{
    static class SessionManager
    {
        public static string CurrentUser { get; set; } = string.Empty;
        public static int CurrentUserId { get; set; } = -1;
        public static bool IsLoggedIn { get; set; } = false;
        public static void ClearSession()
        {
            CurrentUser = string.Empty;
            CurrentUserId = -1;
            IsLoggedIn = false;
        }

        public static void SetSession(string username, int userId)
        {
            CurrentUser = username;
            CurrentUserId = userId;
            IsLoggedIn = true;
        }

        public static DifficultyLevel? CurrentDifficulty { get; set; } = null;
        public static string CurrentResult { get; set; } = string.Empty;

        public static void ClearGame()
        {
            CurrentDifficulty = null;
            CurrentResult = string.Empty;
        }

    }
}
