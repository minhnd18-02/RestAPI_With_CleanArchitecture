using Microsoft.EntityFrameworkCore.Storage;
using RestAPIService.Domain.Interfaces;
using RestAPIService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiContext _apiContext;
        private readonly ICategoryRepo _categoryRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IProductRepo _productRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly IUserRepo _userRepo;

        public UnitOfWork(ApiContext apiContext, ICategoryRepo categoryRepo, IOrderRepo orderRepo, IOrderItemRepo orderItemRepo,
            IProductRepo productRepo, IRoleRepo roleRepo, IUserRepo userRepo)

        {
            _apiContext = apiContext;
            _userRepo = userRepo;
            _categoryRepo = categoryRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _roleRepo = roleRepo;
            _orderItemRepo = orderItemRepo;
        }

        public IUserRepo UserRepo => _userRepo;
        public ICategoryRepo CategoryRepo => _categoryRepo;
        public IRoleRepo RoleRepo => _roleRepo;
        public IOrderItemRepo OrderItemRepo => _orderItemRepo;
        public IOrderRepo OrderRepo => _orderRepo;
        public IProductRepo ProductRepo => _productRepo;

        public async Task<int> SaveChangeAsync()
        {
            try
            {
                return await _apiContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception details here
                throw new ApplicationException("An error occurred while saving changes.", ex);
            }
        }
    }
}
