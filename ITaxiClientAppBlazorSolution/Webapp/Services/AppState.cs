
using Base.Service.Contracts;
using Blazored.LocalStorage;

namespace BlazorWebApp.Services
{
    /// <summary>
    /// Stores current App and Data state in memory
    /// </summary>
    public class AppState : IAppState
    {
        private ILocalStorageService _service;
        private const string _authKey = "Token";
        private IAuthResponse? _response;
        #region Auth State

        public AppState(ILocalStorageService service)
        {
            _service = service;
        }

        public bool IsLoggedIn { get => _response?.Token != null; }

        public event Action OnAuthResponseChanged;

        public async Task<IAuthResponse?> GetAuthResponse()
        {
            if (_response == null)
            {
                if (await _service.ContainKeyAsync(_authKey) == true)
                {
                    _response = await _service.GetItemAsync<AuthResponse?>(_authKey);
                }
            }
            return _response;
        }

        public async Task SetAuthResponse(IAuthResponse response)
        {
            _response = response;
            await _service.SetItemAsync(_authKey, response);
            OnAuthResponseChanged?.Invoke();
        }

        public async Task ResetAuthState()
        {
            _response = null;
            await _service.RemoveItemAsync(_authKey);
        }

        #endregion Auth State

    }
}
