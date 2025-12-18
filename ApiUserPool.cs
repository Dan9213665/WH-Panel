using System;
using System.Collections.Generic;

namespace WH_Panel
{
    internal static class ApiUserPool
    {
        private static List<(string Username, string Password)> _apiUsers = new();
        private static int _currentIndex = 0;
        private static readonly object _locker = new();

        public static void Initialize(List<(string Username, string Password)> users)
        {
            if (users == null || users.Count == 0)
                throw new ArgumentException("User list cannot be null or empty.");

            _apiUsers = users;
        }

        public static (string Username, string Password) GetNextApiUser()
        {
            lock (_locker)
            {
                var user = _apiUsers[_currentIndex];
                _currentIndex = (_currentIndex + 1) % _apiUsers.Count;
                return user;
            }
        }

        // new property
        public static int Count => _apiUsers.Count;
    }
}
