// Controllers/ExternalApiController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using MyReactApp.Data;
using MyReactApp.Models;

namespace MyReactApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExternalApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _context;
        private readonly ILogger<ExternalApiController> _logger;

        public ExternalApiController(IHttpClientFactory httpClientFactory, AppDbContext context, ILogger<ExternalApiController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserData(int userId)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://reqres.in/api/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error retrieving user data. Status code: {response.StatusCode}");
                return BadRequest();
            }

            var userDataJson = await response.Content.ReadAsStringAsync();
            var userData = JsonConvert.DeserializeObject<UserData>(userDataJson);

          
            _context.UserDatas.Add(userData);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User data retrieved successfully. User ID: {userId}");

            return Ok(userDataJson);
        }
    }
}
