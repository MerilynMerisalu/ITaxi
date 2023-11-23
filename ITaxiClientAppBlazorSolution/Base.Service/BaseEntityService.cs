
using Base.Service.Contracts;
using BlazorWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    public abstract class BaseEntityService<TEntity, TKey> : MVCBaseService, IBaseEntityService<TEntity, TKey>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        protected BaseEntityService(HttpClient client, IAppState appState) : base(client, appState)
        {
        }

        public virtual async Task<IEnumerable<TEntity?>> GetAllAsync()
        {
            return await Client.GetFromJsonAsync<IEnumerable<TEntity?>>(GetEndpointUrl());
        }

        public virtual async Task<TEntity?> GetEntityByIdAsync(Guid id)
        {
            return await Client.GetFromJsonAsync<TEntity?>(GetEndpointUrl()  + id);
        }

        public virtual async Task<TEntity?> RemoveEntityAsync(Guid id)
        {
            return await Client.DeleteFromJsonAsync<TEntity?>(GetEndpointUrl() + id);
        }
    }
}
