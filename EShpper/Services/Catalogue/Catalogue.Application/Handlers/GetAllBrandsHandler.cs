using AutoMapper;
using Catalogue.Application.Mappers;
using Catalogue.Application.Queries;
using Catalogue.Application.Responses;
using Catalogue.Core.Entities;
using Catalogue.Core.Repositories;
using MediatR;

namespace Catalogue.Application.Handlers
{
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponse>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        public GetAllBrandsHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;

        }
        //public async Task<IList<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        //{
        //    var brandList = await _brandRepository.GetAllBrands();
         var brandResponseList = _mapper.Map<IList<BrandResponse>>(brandList);

        //    //var brandResponseList1 = _mapper.Map<IList<ProductBrand>, IList<BrandResponse>>(brandList.ToList());

        //    return brandResponseList;
        //}

        public async Task<IList<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brandList = await _brandRepository.GetAllBrands();

          
            var brandResponseList = ProductMapper.Mapper.Map<IList<ProductBrand>, IList<BrandResponse>>(brandList.ToList());
            return brandResponseList;
        }
    }
}
