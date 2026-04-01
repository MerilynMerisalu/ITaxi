namespace Base.Contracts.ViewModels
{
    public interface IShowHideItem
    {
        Guid Id { get; }
        bool IsIgnored { get; }
    }
}
