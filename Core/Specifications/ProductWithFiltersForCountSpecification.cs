using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecParams prodParams)
            : base(x => 
                (string.IsNullOrEmpty(prodParams.Search) || x.Name.ToLower().Contains(prodParams.Search)) &&
                (!prodParams.BrandId.HasValue || x.ProductBrandId == prodParams.BrandId) &&
                (!prodParams.TypeId.HasValue || x.ProductTypeId == prodParams.TypeId))
        {
        }
    }
}