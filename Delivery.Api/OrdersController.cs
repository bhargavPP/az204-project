using Delivery.Api.Models;
using Delivery.Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;
        private readonly BlobService _blobService;
        private readonly QueueService _queueService;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(OrderService service, BlobService blobService, QueueService queueService, ILogger<OrdersController> logger)
        {
            _service = service;
            _blobService = blobService;
            _queueService = queueService;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            var result = _service.Create(order);
            _logger.LogInformation("Order created with ID: {OrderId}", result.Id);
            await _queueService.SendMessageAsync("orders",result);
            _logger.LogInformation("Order sent with message ID: {OrderId}", result.Id);
            return Ok(result);
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, BlobService blobService)
        {
            _logger.LogInformation("Upload");
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var url = await blobService.UploadAsync(file);
            return Ok(new { Url = url }); // Returning an object is better practice than a raw string
        }
    }
}
