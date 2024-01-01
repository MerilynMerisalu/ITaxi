
using Base.Service.Contracts;
using BlazorWebApp.Services;
using System.Net.Http.Json;
using System.Net.Http;


namespace Webapp.Services
{
    public class ITaxiAuthTokenHandler(IAppState appState) : DelegatingHandler
    {
        private readonly IAppState _appState = appState;


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authentication = await _appState.GetAuthResponse();

            if (authentication != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authentication.Token);
            }
            return await base.SendAsync(request, cancellationToken);
        }


    }
}
