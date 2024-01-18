﻿
using Base.Service.Contracts;
using BlazorWebApp.Services;
using Public.App.DTO.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service
{
    public abstract class BaseEntityService<TEntity, TKey> : MVCBaseService, IBaseEntityService<TEntity, TKey>
        where TEntity : Entity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected BaseEntityService(HttpClient client, IAppState appState) : base(client, appState)
        {
        }

        public async Task<List<TEntity?>> AddEntitiesAsync(List<TEntity?> entities)
        {
            var response = await Client.PostAsJsonAsync<List<TEntity?>>(GetEndpointUrl(), entities);
            var result = await response.Content.ReadFromJsonAsync<List<TEntity?>>();
            return result.ToList();
        }

        public async Task<TEntity> AddEntity(TEntity entity)
        {
            var response = await Client.PostAsJsonAsync<TEntity?>(GetEndpointUrl(),entity);
            var result = await response.Content.ReadFromJsonAsync<TEntity>();
            return result;
        }

        public virtual async Task<IEnumerable<TEntity?>> GetAllAsync()
        {
            return await Client.GetFromJsonAsync<IEnumerable<TEntity?>>(GetEndpointUrl());
        }

        public virtual async Task<TEntity?> GetEntityByIdAsync(Guid id)
        {
            return await Client.GetFromJsonAsync<TEntity?>(GetEndpointUrl()  + id);
        }

        public virtual async Task RemoveEntityAsync(Guid id)
        {
            var response = await Client.DeleteAsync(GetEndpointUrl() + id);
            if(response.IsSuccessStatusCode == false)
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }
            //return await Client.DeleteFromJsonAsync<TEntity?>(GetEndpointUrl() + id);
        }

        public async Task UpdateEntityAsync(TEntity entity)
        {
            var response = await Client.PutAsJsonAsync<TEntity?>(GetEndpointUrl() + entity.Id, entity);
           
            
        }
    }
}
