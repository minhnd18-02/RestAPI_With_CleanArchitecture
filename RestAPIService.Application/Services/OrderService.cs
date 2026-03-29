using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestAPIService.Application.IServices;
using RestAPIService.Application.ViewModels;
using RestAPIService.Application.ViewModels.OrderModel;
using RestAPIService.Domain.Entities;
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
        public async Task<ServiceReponse<OrderResponse>> AddOrder(CreateOrderRequest orderRequest)
        {
            var response = new ServiceReponse<OrderResponse>();
            try
            {
                var user = await _unitOfWork.UserRepo.FindEntityAsync(u => u.Id == orderRequest.UserId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                var order = new Order
                {
                    UserId = orderRequest.UserId,
                    Status = OrderStatus.Pending,
                    OrderItems = new List<OrderItem>()
                };

                decimal total = 0;

                // 🔥 Loop từng item
                foreach (var item in orderRequest.Items)
                {
                    var product = await _unitOfWork.ProductRepo.FindEntityAsync(p => p.Id == item.ProductId);

                    if (product == null)
                    {
                        response.Success = false;
                        response.Message = $"ProductId {item.ProductId} not found";
                        return response;
                    }

                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price // 🔥 QUAN TRỌNG
                    };

                    total += product.Price * item.Quantity;

                    order.OrderItems.Add(orderItem);
                }

                // 🔹 set total
                order.TotalAmount = total;

                await _unitOfWork.OrderRepo.AddAsync(order);

                var orderNew = await _unitOfWork.OrderRepo.Filter()
                    .Where(o => o.Id == order.Id)
                    .Include(i => i.OrderItems)
                    .FirstOrDefaultAsync();

                response.Success = true;
                response.Message = "Order retrieve successfull";
                response.Data = _mapper.Map<OrderResponse>(orderNew);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceReponse<IEnumerable<OrderResponse>>> GetOrderByUserId(int userId)
        {
            var response = new ServiceReponse<IEnumerable<OrderResponse>>();
            try
            {
                var checkUserId = await _unitOfWork.UserRepo.FindEntityAsync(u => u.Id.Equals(userId));
                if (checkUserId == null)
                {
                    response.Success = false;
                    response.Message = "User Id was not found";
                    return response;
                }

                var checkOrder = await _unitOfWork.OrderRepo.Filter()
                    .Where(u => u.UserId == userId)
                    .Include(o => o.OrderItems)
                    .ToListAsync();

                if (checkOrder == null)
                {
                    response.Success = false;
                    response.Message = "User has no order";
                    return response;
                }

                response.Success = true;
                response.Message = "Order retreive successfully";
                response.Data = _mapper.Map<IEnumerable<OrderResponse>>(checkOrder);
                ;

            }catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceReponse<OrderResponse>> UpdateOrder(UpdateOrderRequest request)
        {
            var response = new ServiceReponse<OrderResponse>();

            try
            {
                // 🔹 Lấy order + items
                var order = await _unitOfWork.OrderRepo.Filter()
                    .Where(o => o.Id == request.OrderId)
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    response.Success = false;
                    response.Message = "Order not found";
                    return response;
                }

                // 🔥 Rule: không cho update nếu đã Shipping
                if (order.Status != OrderStatus.Pending && order.Status != OrderStatus.Confirmed)
                {
                    response.Success = false;
                    response.Message = "Cannot update order in current status";
                    return response;
                }

                // 🔥 Xóa item cũ
                _unitOfWork.OrderItemRepo.RemoveAsync(order.OrderItems.FirstOrDefault());

                decimal total = 0;
                var newItems = new List<OrderItem>();

                // 🔥 Thêm lại item mới
                foreach (var item in request.Items)
                {
                    var product = await _unitOfWork.ProductRepo.FindEntityAsync(p => p.Id == item.ProductId);

                    if (product == null)
                    {
                        response.Success = false;
                        response.Message = $"ProductId {item.ProductId} not found";
                        return response;
                    }

                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = product.Price // 🔥 luôn lấy từ DB
                    };

                    total += product.Price * item.Quantity;
                    newItems.Add(orderItem);
                }

                // 🔹 cập nhật lại
                order.TotalAmount = total;
                order.OrderItems = newItems;

                await _unitOfWork.OrderRepo.UpdateAsync(order);

                // 🔹 load lại
                var orderNew = await _unitOfWork.OrderRepo.Filter()
                    .Where(o => o.Id == order.Id)
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync();

                response.Success = true;
                response.Message = "Order updated successfully";
                response.Data = _mapper.Map<OrderResponse>(orderNew);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
