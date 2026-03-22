using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public ICategoryRepo CategoryRepo { get; }
        public IOrderRepo OrderRepo { get; }
        public IOrderItemRepo OrderItemRepo { get; }
        public IProductRepo ProductRepo { get; }
        public IRoleRepo RoleRepo { get; }
        public IUserRepo UserRepo { get; }
        public Task<int> SaveChangeAsync();
    }
}
