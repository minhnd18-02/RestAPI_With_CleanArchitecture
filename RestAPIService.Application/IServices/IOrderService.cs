using RestAPIService.Application.ViewModels;
using RestAPIService.Application.ViewModels.OrderModel;
using RestAPIService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.IServices
{
    public interface IOrderService
    {
        Task<ServiceReponse<OrderResponse>> AddOrder(CreateOrderRequest orderRequest);
        Task<ServiceReponse<IEnumerable<OrderResponse>>> GetOrderByUserId(int userId);
        Task<ServiceReponse<OrderResponse>> UpdateOrder(UpdateOrderRequest request);
    }
}
