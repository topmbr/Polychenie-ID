using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using ProjectNamespace.Models;
using receive_ID.servis;
using ProjectNamespace.Services;

namespace ProjectNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IDbService _dbService;
        private readonly HttpClient _httpClient;

        public UserController(IDbService dbService, IHttpClientFactory httpClientFactory)
        {
            _dbService = dbService;
            _httpClient = httpClientFactory.CreateClient(); // Создаем экземпляр HttpClient
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                // Отправляем запрос на внешний API для получения данных о пользователе
                var response = await _httpClient.GetAsync($"https://reqres.in/api/users/{id}");

                // Проверяем успешность запроса
                if (response.IsSuccessStatusCode)
                {
                    // Чтение содержимого ответа как строки
                    var contentString = await response.Content.ReadAsStringAsync();

                    // Десериализация строки в объект типа User
                    var externalUserData = JsonConvert.DeserializeObject<ExternalUserData>(contentString); // Подставьте правильный тип User

                    var user = new User
                    {
                        Id = externalUserData.data.id,
                        Email = externalUserData.data.email,
                        FirstName = externalUserData.data.first_name,
                        LastName = externalUserData.data.last_name,
                        Avatar = externalUserData.data.avatar
                    };

                    // Сохраняем пользователя в базу данных
                    await _dbService.AddUser(user);

                    // Возвращаем данные пользователя в формате JSON
                    return Ok(user);
                }
                else
                {
                    // Если запрос не успешен, возвращаем ошибку
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Если произошла ошибка, возвращаем статус 500
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
