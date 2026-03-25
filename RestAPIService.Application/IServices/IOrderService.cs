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
        Task<ServiceReponse<OrderResponse>> GetOrderByUserId(int userId);
        Task<ServiceReponse<OrderResponse>> UpdateStatusOrder(OrderStatus status);
    }
}
