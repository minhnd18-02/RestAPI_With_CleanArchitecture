using RestAPIService.Domain.Entities;
using RestAPIService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.ViewModels.OrderModel
{
    public class CreateOrderRequest
    {
        public int UserId { get; set; }

        public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
    }
}
