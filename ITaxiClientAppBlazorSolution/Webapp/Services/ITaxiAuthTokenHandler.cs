
using Base.Service.Contracts;
using BlazorWebApp.Services;
using System.Net.Http.Json;
using System.Net.Http;
using Microsoft.Extensions.Http;


namespace Webapp.Services
{
    public class ITaxiAuthTokenHandler(IAppState appState, IHttpClientFactory client) : DelegatingHandler
    {
        private readonly IAppState _appState = appState;
        private readonly IHttpClientFactory _httpClient = client;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authentication = await _appState.GetAuthResponse();

            if (authentication != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authentication.Token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
        public async Task<AuthResponse> AuthenticateToken(string email, string password)
        {
            AuthResponse authResponse = new AuthResponse();

            var client = _httpClient.CreateClient("Auth");

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            var response = await client.PostAsJsonAsync($"{client.BaseAddress!.ToString().TrimEnd('/')}/identity/account/login", new { Email = email, Password = password });
            if (response.IsSuccessStatusCode)
            {
                authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

            }

            return authResponse!;
        }

    }
}
