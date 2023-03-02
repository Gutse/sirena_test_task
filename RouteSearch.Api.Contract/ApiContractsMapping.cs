using Mapster;
using RouteSearch.Contract;

namespace RouteSearch.Api.Contract
{
    public class ApiContractsMapping: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            TypeAdapterConfig<SearchRequest, SearchDto>.NewConfig()
                .ConstructUsing(r => new SearchDto(r.Origin!, r.Destination!, r.OriginDateTime));

            TypeAdapterConfig<SearchResultDto, SearchResponse>.NewConfig();
        }
    }
}