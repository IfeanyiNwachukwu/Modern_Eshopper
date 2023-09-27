using Catalogue.Application.Responses;
using MediatR;

namespace Catalogue.Application.Queries
{
    public class GetAllTypesQuery : IRequest<IList<TypesResponse>>
    {
    }
}
