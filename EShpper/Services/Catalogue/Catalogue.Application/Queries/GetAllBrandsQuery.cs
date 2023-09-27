
using Catalogue.Application.Responses;
using MediatR;

namespace Catalogue.Application.Queries
{
    public class GetAllBrandsQuery : IRequest<IList<BrandResponse>>
    {
    }
}
