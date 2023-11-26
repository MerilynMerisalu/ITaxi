namespace Base.Service.Contracts
{
    public interface IBaseEntityService<TEntity, TKey> where TEntity : class
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Get the base URL from the underlying HttpClient
        /// </summary>
        /// <returns></returns>
        string GetBaseUrl();
        /// <summary>
        /// Get the URL for the Endpoint on the API that serves the <typeparamref name="TEntity"/> resources
        /// </summary>
        /// <returns></returns>
        string GetEndpointUrl();
        /// <summary>
        /// Get all <typeparamref name="TEntity"/> resources from the specified <paramref name="endpoint"/>
        /// </summary>
        /// <remarks><paramref name="endpoint"/> will be appended to the base address of the underlying HttpClient</remarks>
        /// <param name="endpoint">The endpoint on the API to load the resources</param>
        /// <returns></returns>
        Task<IEnumerable<TEntity?>> GetAllAsync();
        Task<TEntity?>GetEntityByIdAsync(Guid id);
        Task RemoveEntityAsync(Guid id);

    }
}
