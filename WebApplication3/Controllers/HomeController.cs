using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApplication3.Interfaces;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IDataService _dataService;

        public HomeController(ILoggerService logger, IDataService dataService)
        {
            _logger = logger;
            _dataService = dataService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {

                var user = await _dataService.GetUser(id);

                if (user != null)
                {

                    Log.Information("User data retrieved: {@User}", user);

                    return Ok(user);
                }
                else
                {

                    return NotFound();
                }
            }
            catch (Exception ex)
            {

               Log.Error(ex, "Error retrieving user data");

                return StatusCode(500, "Internal server error");
            }
        }
    }
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
    }
}
