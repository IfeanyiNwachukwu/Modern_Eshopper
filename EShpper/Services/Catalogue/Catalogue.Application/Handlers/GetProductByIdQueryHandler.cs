using Catalogue.Application.Mappers;
using Catalogue.Application.Queries;
using Catalogue.Application.Responses;
using Catalogue.Core.Repositories;
using MediatR;

namespace Catalogue.Application.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetProduct(request.Id);
            var productResponse = ProductMapper.Mapper.Map<ProductResponse>(product);
            return productResponse;
        }
    }
}
