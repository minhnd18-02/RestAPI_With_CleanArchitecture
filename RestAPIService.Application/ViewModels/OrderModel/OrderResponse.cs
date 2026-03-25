using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.ViewModels.OrderModel
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
    }
}
