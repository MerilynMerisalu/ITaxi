using Base.Service.Contracts;
using System.Net.Http.Json;
using System.Net.NetworkInformation;

namespace BlazorWebApp.Services
{
    
    public abstract class MVCBaseService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppState _appState;
        public MVCBaseService(HttpClient client, IAppState appState)
        {
            _httpClient = client;
            _appState = appState;
        }

        /// <summary>
        /// The is the API Endpoint for this resource
        /// </summary>
        protected abstract string EndpointUri { get; }

        public string GetBaseUrl() => _httpClient.BaseAddress!.ToString().TrimEnd('/');
        public string GetEndpointUrl() => $"{GetBaseUrl().TrimEnd('/')}/{EndpointUri.TrimStart('/')}";
        public HttpClient Client { get => _httpClient; }

        //public async Task<bool> Login(string username, string password)
        //{
        //    var token = await AuthenticateToken(username, password);
        //    _appState.AuthResponse = token;
        //    return token?.Token != null;
        //}

        //private async Task<OAuthResponse> AuthenticateToken(string username, string password)
        //{
        //    OAuthResponse authResponse = new OAuthResponse();
        //    var client = this.Client;
        //    client.DefaultRequestHeaders.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
        //    Dictionary<string, string> data = new Dictionary<string, string>
        //    {
        //        { "grant_type", "password" },
        //        { "username", username },
        //        { "password", password }
        //    };

        //    var response = await client.PostAsync($"{GetBaseUrl().TrimEnd('/')}/token", new System.Net.Http.FormUrlEncodedContent(data));
        //    if (response.IsSuccessStatusCode)
        //    {
        //        authResponse = await response.Content.ReadFromJsonAsync<OAuthResponse>();
               
        //    }

        //    return authResponse;
        //}

    }
}
