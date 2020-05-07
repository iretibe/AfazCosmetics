using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spc)
        {
            var query = inputQuery;

            if(spc.Criteria != null) 
            {
                query = query.Where(spc.Criteria);
            }

            query = spc.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}