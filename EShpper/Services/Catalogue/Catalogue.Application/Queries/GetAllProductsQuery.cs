using Catalogue.Application.Responses;
using MediatR;

namespace Catalogue.Application.Queries
{
    public class GetAllProductsQuery : IRequest<IList<ProductResponse>>
    {
    }
}
