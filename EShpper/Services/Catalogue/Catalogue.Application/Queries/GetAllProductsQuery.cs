using Catalogue.Application.Responses;
using Catalogue.Core.Specs;
using MediatR;

namespace Catalogue.Application.Queries
{
    public class GetAllProductsQuery : IRequest<Pagination<ProductResponse>>
    {
        public CatalogueSpecParams CatalogueSpecParams { get; set; }

        public GetAllProductsQuery(CatalogueSpecParams catalogueSpecParams)
        {
            CatalogueSpecParams = catalogueSpecParams;
        }
    }
}
