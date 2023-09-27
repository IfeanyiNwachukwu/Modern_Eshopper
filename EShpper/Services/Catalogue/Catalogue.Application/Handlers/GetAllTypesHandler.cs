using Catalogue.Application.Mappers;
using Catalogue.Application.Queries;
using Catalogue.Application.Responses;
using Catalogue.Core.Repositories;
using MediatR;

namespace Catalogue.Application.Handlers
{
    public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponse>>
    {
        private readonly ITypesRepository _typesRepository;

        public GetAllTypesHandler(ITypesRepository typesRepository)
        {
            _typesRepository = typesRepository;
        }
        public async Task<IList<TypesResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var typesList = await _typesRepository.GetAllTypes();
            var typesResponseList = ProductMapper.Mapper.Map<IList<TypesResponse>>(typesList);
            return typesResponseList;
        }
    }
}
