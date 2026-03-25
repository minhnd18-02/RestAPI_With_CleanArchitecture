using AutoMapper;
using RestAPIService.Application.IServices;
using RestAPIService.Application.ViewModels;
using RestAPIService.Application.ViewModels.OrderModel;
using RestAPIService.Domain.Enums;
using RestAPIService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<ServiceReponse<OrderResponse>> AddOrder(CreateOrderRequest orderRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceReponse<OrderResponse>> GetOrderByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceReponse<OrderResponse>> UpdateStatusOrder(OrderStatus status)
        {
            throw new NotImplementedException();
        }
    }
}
