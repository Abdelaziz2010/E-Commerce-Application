﻿using Ecom.Application.DTOs.Order;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Application.Services.Interfaces;
using Ecom.Domain.Entities.Orders;
using Ecom.Domain.Entities.Product;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
namespace Ecom.Infrastructure.Implementation.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;
        public OrderService(IUnitOfWork unitOfWork, AppDbContext context,IMapper mapper, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(OrderDTO orderDTO, string buyerEmail)
        {
            
            var cart = await _unitOfWork.CartRepository.GetCartAsync(orderDTO.CartId);

            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (var item in cart.CartItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                
                var orderItem = new OrderItem(product.Id, item.Image, product.Name, item.Price, item.Quantity);

                orderItems.Add(orderItem);
            }

            var deliveryMethod = await _context.DeliveryMethods.FirstOrDefaultAsync(x => x.Id == orderDTO.DeliveryMethodId);

            var subTotal = orderItems.Sum(x => x.Price * x.Quantity);

            var shippingAddress = _mapper.Map<ShippingAddress>(orderDTO.ShippingAddress);

            var existingOrder = await _context.Orders
                .Where(x => x.PaymentIntentId == cart.PaymentIntentId)
                .FirstOrDefaultAsync();

            if (existingOrder is not null)
            {
                _context.Orders.Remove(existingOrder);
                await _paymentService.CreateOrUpdatePaymentAsync(cart.PaymentIntentId, deliveryMethod.Id);

            }

            var order = new Order()
            {
                BuyerEmail = buyerEmail,
                ShippingAddress = shippingAddress,
                DeliveryMethod = deliveryMethod,
                OrderItems = orderItems,
                SubTotal = subTotal,
                PaymentIntentId = cart.PaymentIntentId
            };

            await _context.Orders.AddAsync(order);
           
            await _context.SaveChangesAsync();
            
            await _unitOfWork.CartRepository.DeleteCartAsync(orderDTO.CartId);  // Clear the cart after order creation

            return order;
        }

        public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var orders = await _context.Orders
                .Where(x => x.BuyerEmail == buyerEmail)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.OrderItems)
                .AsNoTracking()
                .ToListAsync();

            var result = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
            
            return result;
        }

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await _context.Orders
                .Where(x => x.BuyerEmail == buyerEmail && x.Id == id)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.OrderItems)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var result = _mapper.Map<OrderToReturnDTO>(order);

            return result;
        }

        public async Task<IReadOnlyList<DeliveryMethodDTO>> GetDeliveryMethodsAsync()
        {

            var deliveryMethods = await _context.DeliveryMethods.AsNoTracking().ToListAsync();
            
            var result = _mapper.Map<IReadOnlyList<DeliveryMethodDTO>>(deliveryMethods);

            return result;
        }
    }
}
