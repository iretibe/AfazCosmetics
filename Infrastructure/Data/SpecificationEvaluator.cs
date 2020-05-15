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

            if(spc.OrderBy != null) 
            {
                query = query.OrderBy(spc.OrderBy);
            }

            if(spc.OrderByDescending != null) 
            {
                query = query.OrderByDescending(spc.OrderByDescending);
            }

            if(spc.IsPagingEnabled) 
            {
                query = query.Skip(spc.Skip).Take(spc.Take);
            }

            query = spc.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}