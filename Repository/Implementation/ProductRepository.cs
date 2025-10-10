using ERP_System_Project.Models;
using ERP_System_Project.Models.Inventory;
using ERP_System_Project.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ERP_System_Project.Repository.Implementation
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(Erpdbcontext db) : base(db) { }

        public override Task<TResult?> GetAsync<TResult>(
        Expression<Func<Product, TResult>> selector,
        Expression<Func<Product, bool>> filter,
        params Expression<Func<Product, object>>[] Includes) where TResult : class
        {
            IQueryable<Product> query = _dbSet;

            query = query.Where(filter);

            foreach (var include in Includes)
                query = query.Include(include);

            query = query
                .Include(p => p.Attributes)
                    .ThenInclude(a => a.ProductAttribute)
                .Include(p => p.CustomerReviews)
                    .ThenInclude(cr => cr.Customer);

            return query.Select(selector).FirstOrDefaultAsync();
        }

    }
}
