using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.ViewModels.OrderModel
{
    public class UpdateOrderRequest
    {
        public int OrderId { get; set; }
        public List<OrderItemRequest> Items { get; set; }
    }
}
