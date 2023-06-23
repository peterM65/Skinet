using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
        private readonly IGenericRepository<ProductType> _productTypesRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo,
           IGenericRepository<ProductBrand> productBrandsRepo,
           IGenericRepository<ProductType> productTypesRepo,
           IMapper mapper)
        {
            _productsRepo = productsRepo;
            _productBrandsRepo = productBrandsRepo;
            _productTypesRepo = productTypesRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypeAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);
            var totalItems = await _productsRepo.CountAsync(countSpec);
            var products = await _productsRepo.ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);
            var returnDto = new Pagination<ProductToReturnDto>(productParams.PageIndex,
                productParams.PageSize, totalItems, data);
            return Ok(returnDto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypeAndBrandsSpecification(id);
            var product= await _productsRepo.GetEntityWithSpec(spec);
            var returnDto = _mapper.Map<Product, ProductToReturnDto>(product);
            if (product == null) return NotFound(new ApiResponse(404));
            return returnDto;
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var productBrands = await _productBrandsRepo.ListAllAsync();

            return Ok(productBrands);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var productTypes = await _productTypesRepo.ListAllAsync();

            return Ok(productTypes);
        }
    }
}