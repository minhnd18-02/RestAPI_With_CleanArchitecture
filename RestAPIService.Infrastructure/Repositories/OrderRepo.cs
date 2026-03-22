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
    public class OrderRepo : GenericRepo<Order>, IOrderRepo
    {
        public OrderRepo(ApiContext context) : base(context)
        {
        }
    }
}
