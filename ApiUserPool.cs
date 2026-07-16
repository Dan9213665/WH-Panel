using System;
using System.Collections.Generic;


internal static class ApiUserPool
{
    private static List<(string Username, string Password)> _apiUsers = new();
    private static int _currentIndex = 0;
    private static readonly object _locker = new();

    public static void Initialize(List<(string Username, string Password)> users)
    {
        if (users == null || users.Count == 0)
            throw new ArgumentException("User list cannot be null or empty.");

        // --- TEMPORARY BLACKLIST FOR APRIL ---
        var blacklist = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            //"6ADFFD01B1B04B10A4F8FD7BCA8631D8","7D4B614B3FD645F584C8661B813B5E98"//,"AEF3B8E8189A481786598CCCFD16A56A" // Add any other exhausted users here
            "AEF3B8E8189A481786598CCCFD16A56A"
        };

        lock (_locker)
        {
            // Only add users who ARE NOT in the blacklist
            _apiUsers = users.FindAll(u => !blacklist.Contains(u.Username));

            // Safety check: if you accidentally blacklist everyone
            if (_apiUsers.Count == 0)
                _apiUsers = users;

            //foreach (var user in _apiUsers)
            //{
            //    MessageBox.Show($"Added API user: {user.Username}");
            //}
        }
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

    public static int Count => _apiUsers.Count;
}