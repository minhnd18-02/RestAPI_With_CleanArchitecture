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
    public class OrderItemRepo : GenericRepo<OrderItem>, IOrderItemRepo
    {
        public OrderItemRepo(ApiContext context) : base(context)
        {
        }
    }
}
