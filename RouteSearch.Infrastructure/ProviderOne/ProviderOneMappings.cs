using Mapster;
using RouteSearch.Contract;
using System;

namespace RouteSearch.Infrastructure.ProviderOne
{
    public class ProviderOneMappings: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<ProviderOneRoute, Route>.NewConfig()
                .Map(r => r.Id, () => Guid.NewGuid().ToString())
                ;
        }
    }
}