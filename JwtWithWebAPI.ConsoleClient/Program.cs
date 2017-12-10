using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace JwtWithWebAPI.ConsoleClient
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //Note: First you should run the `JwtWithWebAPI` project and then run the `ConsoleClient` project.

            var baseAddress = "http://localhost:9577/";
            var token = LoginAsync(baseAddress: baseAddress, username: "Vahid", password: "1234").GetAwaiter().GetResult();
            CallProtectedApiAsync(baseAddress: baseAddress, token: token).GetAwaiter().GetResult();

            Console.WriteLine("\nPress a key ...");
            Console.Read();
        }

        private static async Task<Token> LoginAsync(string baseAddress, string username, string password)
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", username },
                    { "password", password }
                };
                var tokenResponse = await client.PostAsync($"{baseAddress}/login", new FormUrlEncodedContent(form));
                if (tokenResponse.StatusCode == HttpStatusCode.OK)
                {
                    var token = await tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() });
                    Console.WriteLine("AccessToken issued is: {0}", token.AccessToken);
                    Console.WriteLine("RefreshToken issued is: {0}", token.RefreshToken);
                    return token;
                }

                throw new UnauthorizedAccessException(tokenResponse.ToString());
            }
        }

        private static async Task CallProtectedApiAsync(string baseAddress, Token token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                var response = await client.GetAsync("/api/MyProtectedApi");
                var message = await response.Content.ReadAsStringAsync();
                Console.WriteLine("URL response : " + message);
            }
        }
    }
}
