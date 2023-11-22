
using Base.Service.Contracts;

namespace BlazorWebApp.Services
{
    /// <summary>
    /// Stores current App and Data state in memory
    /// </summary>
    public class AppState : IAppState
    {
        #region Auth State

        public bool IsLoggedIn { get => AuthResponse?.Token != null; }
        public IAuthResponse AuthResponse 
        { 
            get => _authResponse;
            set
            {
                _authResponse = value;
                OnAuthResponseChanged?.Invoke();
            }
        }
        private IAuthResponse _authResponse;
        public event Action OnAuthResponseChanged;

        #endregion Auth State

    }
}
