using Delivery.Api.Models;
using Delivery.Api.Service;
using Microsoft.AspNetCore.Http;
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
        public OrdersController(OrderService service, BlobService blobService, QueueService queueService)
        {
            _service = service;
            _blobService = blobService;
            _queueService = queueService;
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
            await _queueService.SendMessageAsync("orders",result);
            return Ok(result);
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, BlobService blobService)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var url = await blobService.UploadAsync(file);
            return Ok(new { Url = url }); // Returning an object is better practice than a raw string
        }
    }
}
