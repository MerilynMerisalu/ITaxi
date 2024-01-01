using Base.Service;
using Base.Service.Contracts;
using Public.App.DTO.v1;
using Public.App.DTO.v1.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ITaxi.Service
{
    public interface IRegisterService
    {
        Task<RegisterCustomerJWTResponse?> RegisterCustomer(RegisterCustomer customer);
    }

    public class RegisterService : BaseEntityService<Register, Guid>, IRegisterService
    {
        public RegisterService(IHttpClientFactory clientProvider, IAppState appState) :
    base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "identity/Account/";
        protected virtual string RegisterEndPointUri => Client.BaseAddress + EndpointUri +  "RegisterCustomerDTO";

        public async Task<RegisterCustomerJWTResponse?> RegisterCustomer(RegisterCustomer customer)
        {
            var response = await Client.PostAsJsonAsync(RegisterEndPointUri, customer);
            var result = await response.Content.ReadFromJsonAsync<RegisterCustomerJWTResponse>();
            return result;
        }
    }
}
