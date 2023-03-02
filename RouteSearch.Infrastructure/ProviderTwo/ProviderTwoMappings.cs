using Mapster;
using RouteSearch.Contract;
using System;

namespace RouteSearch.Infrastructure.ProviderTwo
{
    public class ProviderTwoMappings: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<ProviderTwoRoute, Route>.NewConfig()
                .Map(r => r.Id, () => Guid.NewGuid().ToString())
                ;
        }
    }
}