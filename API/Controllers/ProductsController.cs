using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;

namespace API.Controllers
{    
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productbandsRepo;
        private readonly IGenericRepository<ProductType> _producttypesRepo;
        private readonly IMapper _mapper;

        //public IProductRepository _repo;
        // public ProductsController(IProductRepository repo)
        // {
        //     _repo = repo;
        // }

        public ProductsController(IGenericRepository<Product> productsRepo,
            IGenericRepository<ProductBrand> productbandsRepo, 
            IGenericRepository<ProductType> producttypesRepo, IMapper mapper)
        {
            _productsRepo = productsRepo;
            _productbandsRepo = productbandsRepo;
            _producttypesRepo = producttypesRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams prodParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(prodParams);

            var countSpec = new ProductWithFiltersForCountSpecification(prodParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            
            return Ok(new Pagination<ProductToReturnDto>(prodParams.PageIndex, prodParams.PageIndex, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var products = await _productsRepo.GetEntityWithSpec(spec);

            if(products == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(products);
        }  

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            // return Ok(await _repo.GetProductBrandsAsync());
            return Ok(await _productbandsRepo.ListAllAsync());
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _producttypesRepo.ListAllAsync());
        }

        // [HttpGet]
        // public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams prodParams)
        // {
        //     var spec = new ProductsWithTypesAndBrandsSpecification(prodParams);
        //     var products = await _productsRepo.ListAsync(spec);
            
        //     return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        // }

        // [HttpGet]
        // public async Task<ActionResult<List<Product>>> GetProducts()
        // {
        //     // var products = await _repo.GetProductsAsync();
        //     // var products = await _productsRepo.ListAllAsync();

        //     var spec = new ProductsWithTypesAndBrandsSpecification();
        //     var products = await _productsRepo.ListAsync(spec);
        //     return Ok(products);
        // }

        // [HttpGet]
        // public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        // {
        //     var spec = new ProductsWithTypesAndBrandsSpecification();
        //     var products = await _productsRepo.ListAsync(spec);
        //     // return products.Select(prod => new ProductToReturnDto
        //     // {
        //     //     Id = prod.Id,
        //     //     Name = prod.Name,
        //     //     Description = prod.Description,
        //     //     PictureUrl = prod.PictureUrl,
        //     //     Price = prod.Price,
        //     //     ProductBrand = prod.ProductBrand.Name,
        //     //     ProductType = prod.ProductType.Name
        //     // }).ToList();
        //     return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        // }

        // [HttpGet]
        // public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(string sort, int? brandId, int? typeId)
        // {
        //     var spec = new ProductsWithTypesAndBrandsSpecification(sort, brandId, typeId);
        //     var products = await _productsRepo.ListAsync(spec);
            
        //     return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        // }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<Product>> GetProduct(int id)
        // {
        //     // return await _repo.GetProductByIdAsync(id);
        //     // return await _productsRepo.GetByIdAsync(id);

        //     var spec = new ProductsWithTypesAndBrandsSpecification(id);
        //     return await _productsRepo.GetEntityWithSpec(spec);
        // }

        // [HttpGet("{id}")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        // public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        // {
        //     var spec = new ProductsWithTypesAndBrandsSpecification(id);

        //     var products = await _productsRepo.GetEntityWithSpec(spec);

        //     // return new ProductToReturnDto
        //     // {
        //     //     Id = products.Id,
        //     //     Name = products.Name,
        //     //     Description = products.Description,
        //     //     PictureUrl = products.PictureUrl,
        //     //     Price = products.Price,
        //     //     ProductBrand = products.ProductBrand.Name,
        //     //     ProductType = products.ProductType.Name
        //     // };

        //     if(products == null) return NotFound(new ApiResponse(404));

        //     return _mapper.Map<Product, ProductToReturnDto>(products);
        // }        

        // [HttpGet("Brands")]
        // public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        // {
        //     // return Ok(await _repo.GetProductBrandsAsync());
        //     return Ok(await _productbandsRepo.ListAllAsync());
        // }

        // [HttpGet("Types")]
        // public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        // {
        //     // return Ok(await _repo.GetProductTypesAsync());
        //     return Ok(await _producttypesRepo.ListAllAsync());
        // }
    }
}