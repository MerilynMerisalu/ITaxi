namespace Base.Service.Contracts
{
    public interface IAppState
    {
        #region Auth State

        bool IsLoggedIn { get; }
        IAuthResponse AuthResponse { get; set; }
        event Action OnAuthResponseChanged;

        #endregion Auth State
    }
}