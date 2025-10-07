using example.domain.Interfaces;
using example.infrastructure;
using example.order.consumer.Services.Interfaces;
using example.order.domain.Entities;
using example.order.infrastructure.Enums;
using Newtonsoft.Json;

namespace example.order.consumer.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IReadOnlyRepository<OrderTemp> _orderTempReadOnlyRepository;
        private readonly IRepository<OrderTemp> _orderTempRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;

        public OrderService(Lazy<IUnitOfWork> unitOfWork) : base(unitOfWork)
        {
            _orderTempReadOnlyRepository = unitOfWork.Value.GetReadOnlyRepository<OrderTemp>();
            _orderTempRepository = unitOfWork.Value.GetRepository<OrderTemp>();
            _orderRepository = unitOfWork.Value.GetRepository<Order>();
            _orderDetailRepository = unitOfWork.Value.GetRepository<OrderDetail>();
        }

        public async Task ProcessOrderAsync(Guid orderTempId)
        {
            //Step 1: Fetch the order temp from the database
            var orderTemp = await _orderTempReadOnlyRepository
                .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == orderTempId);

            if (orderTemp == null || string.IsNullOrEmpty(orderTemp.TempData)) return;

            var orderDetails = JsonConvert.DeserializeObject<IEnumerable<OrderDetail>>(orderTemp.TempData)?.ToList();
            var totalAmount = orderDetails?.Sum(od => od.Amount) ?? 0m;
            var totalQuantity = orderDetails?.Sum(od => od.Quantity) ?? 0;

            //Step 2: Validate the data, inventory, etc.

            // Step 3: Create the order in the system and delete the temp record
            await _orderTempRepository.DeleteAsync(orderTemp);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                OrderCode = orderTemp.OrderCode,
                Description = orderTemp.Description,
                TotalAmount = totalAmount,
                TotalQuantity = totalQuantity,
                OrderStatusId = (int)OrderStatusEnum.PENDING,
                CreatedBy = "system"
            };
            await _orderRepository.AddAsync(order);

            orderDetails.ForEach(x =>
            {
                x.OrderId = order.Id;
                x.CreatedBy = "system";
            });

            await _orderDetailRepository.AddRangeAsync(orderDetails);
            await UnitOfWork.SaveChangesAsync();

            // Step 4: Reserve inventory

            // Step 5: Send confirmation email and SSE notification
        }
    }
}
