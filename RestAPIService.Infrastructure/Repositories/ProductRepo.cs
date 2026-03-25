using Microsoft.EntityFrameworkCore;
using RestAPIService.Domain.Entities;
using RestAPIService.Domain.Interfaces;
using RestAPIService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Infrastructure.Repositories
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public ProductRepo(ApiContext context) : base(context)
        {
        }

        public async Task<Product?> GetById(int id)
        {
            return await _context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Product> GetAs()
        {
            return _dbSet
                .AsNoTracking()
                .Include(x => x.Category);
        }
    }
}
