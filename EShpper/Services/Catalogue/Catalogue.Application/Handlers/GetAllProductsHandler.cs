using Catalogue.Application.Mappers;
using Catalogue.Application.Queries;
using Catalogue.Application.Responses;
using Catalogue.Core.Repositories;
using MediatR;

namespace Catalogue.Application.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        public GetAllProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IList<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProducts();
            var productResponseList = ProductMapper.Mapper.Map<IList<ProductResponse>>(productList);

            return productResponseList;
        }
    }
}
