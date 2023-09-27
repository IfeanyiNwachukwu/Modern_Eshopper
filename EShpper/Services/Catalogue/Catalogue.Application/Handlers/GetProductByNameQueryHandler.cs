using Catalogue.Application.Mappers;
using Catalogue.Application.Queries;
using Catalogue.Application.Responses;
using Catalogue.Core.Repositories;
using MediatR;

namespace Catalogue.Application.Handlers
{
    public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByNameQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IList<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            var productList = await _productRepository.GetProductByName(request.Name);
            var productResponseList = ProductMapper.Mapper.Map<IList<ProductResponse>>(productList);
            return productResponseList;
        }
    }
}
