
using Base.Service.Contracts;

namespace Webapp.Services
{
    public class ITaxiAuthTokenHandler : DelegatingHandler
    {
        private readonly IAppState _appState;
        public ITaxiAuthTokenHandler(IAppState appState) 
        {
            _appState = appState;
        }

        protected override async  Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_appState.AuthResponse != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _appState.AuthResponse.Token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
