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

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
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

        // [HttpGet]
        // public async Task<ActionResult<List<Product>>> GetProducts()
        // {
        //     // var products = await _repo.GetProductsAsync();
        //     // var products = await _productsRepo.ListAllAsync();

        //     var spec = new ProductsWithTypesAndBrandsSpecification();
        //     var products = await _productsRepo.ListAsync(spec);
        //     return Ok(products);
        // }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            // return products.Select(prod => new ProductToReturnDto
            // {
            //     Id = prod.Id,
            //     Name = prod.Name,
            //     Description = prod.Description,
            //     PictureUrl = prod.PictureUrl,
            //     Price = prod.Price,
            //     ProductBrand = prod.ProductBrand.Name,
            //     ProductType = prod.ProductType.Name
            // }).ToList();
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<Product>> GetProduct(int id)
        // {
        //     // return await _repo.GetProductByIdAsync(id);
        //     // return await _productsRepo.GetByIdAsync(id);

        //     var spec = new ProductsWithTypesAndBrandsSpecification(id);
        //     return await _productsRepo.GetEntityWithSpec(spec);
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var products = await _productsRepo.GetEntityWithSpec(spec);

            // return new ProductToReturnDto
            // {
            //     Id = products.Id,
            //     Name = products.Name,
            //     Description = products.Description,
            //     PictureUrl = products.PictureUrl,
            //     Price = products.Price,
            //     ProductBrand = products.ProductBrand.Name,
            //     ProductType = products.ProductType.Name
            // };

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
            // return Ok(await _repo.GetProductTypesAsync());
            return Ok(await _producttypesRepo.ListAllAsync());
        }
    }
}