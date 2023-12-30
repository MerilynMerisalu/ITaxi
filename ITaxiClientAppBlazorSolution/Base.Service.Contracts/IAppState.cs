namespace Base.Service.Contracts
{
    public interface IAppState
    {
        #region Auth State

        bool IsLoggedIn { get; }
        Task<IAuthResponse?> GetAuthResponse();
        Task SetAuthResponse(IAuthResponse response);
        Task ResetAuthState();
        event Action OnAuthResponseChanged;

        #endregion Auth State
    }
}