using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace WH_Panel
{
    public static class ApiHelper
    {
        // New overload for HttpRequestMessage (thread-safe for shared HttpClient)
        public static string AuthenticateClient(HttpRequestMessage request)
        {
            var (username, password) = ApiUserPool.GetNextApiUser();
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            return username;
        }
        public static string AuthenticateClient(HttpClient client)
        {
            var (username, password) = ApiUserPool.GetNextApiUser();

           

            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            
            return username;
        }
    }
}
