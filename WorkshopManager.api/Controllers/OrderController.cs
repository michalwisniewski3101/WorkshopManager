using Microsoft.AspNetCore.Mvc;
using WorkshopManager.api.Model;
using WorkshopManager.api.Repos.Interfaces;

namespace WorkshopManager.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return Ok(orders);
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        // GET: api/Order/{id}
        [HttpGet("ByCode/{code}")]
        public async Task<ActionResult<Order>> GetOrderByCode(string code)
        {
            var order = await _orderRepository.GetOrderByCodeAsync(code);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpGet("ByWorker/{workerId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByWorker(Guid workerId)
        {
            

            var orders = await _orderRepository.GetOrdersByWorkerId(workerId);

            return Ok(orders);
        }
        [HttpGet("ByVehicle/{VehicleId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByVehicle(Guid VehicleId)
        {


            var orders = await _orderRepository.GetOrdersByVehicleId(VehicleId);

            return Ok(orders);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(CreateOrderDto orderDto)
        {
            var newOrder = await _orderRepository.AddOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
        }

        // PUT: api/Order/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            if (!await _orderRepository.UpdateOrderAsync(order))
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            if (!await _orderRepository.DeleteOrderAsync(id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
