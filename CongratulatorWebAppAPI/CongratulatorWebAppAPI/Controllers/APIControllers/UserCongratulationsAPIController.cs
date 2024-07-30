using CongratulatorWebAppAPI.BuisnesObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CongratulatorWebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCongratulationsAPIController : ControllerBase
    {
        UserCongratulationController userCongratulationController = new UserCongratulationController();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var result = userCongratulationController?.GetUserList();
            return result?.Select(s => JsonSerializer.Serialize(s)) ?? new List<string>() { };
        }

        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            return userCongratulationController?.GetUserDetails(id)?.ToString() ?? "";
        }

        [EnableCors("CorsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Post(string message, string type)
        {
            try
            {
                var userCongratulation = new UserCongratulation() { Message = message, Type = type };

                await userCongratulationController.CreateAsync(userCongratulation);

                return Ok();
            }
            catch (Exception ex)
            {
                // Обработка различных ошибок и возврат подходящего статус-кода
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                else if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized();
                }
                else
                {
                    // Логирование ошибки
                    //_logger.LogError(ex, "Ошибка при создании пользователя");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Произошла внутренняя ошибка сервера");
                }
            }
        }

        // PUT api/<UserAPIController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, string _message, string _type)
        {
            try
            {
                var userCongratulation = new UserCongratulation(_message, _type);
                await userCongratulationController.UpdateAsync(id, userCongratulation);
                return Ok();
            }
            catch (Exception ex)
            {
                // Обработка различных ошибок и возврат подходящего статус-кода
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                else if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized();
                }
                else
                {
                    // Логирование ошибки
                    //_logger.LogError(ex, "Ошибка при создании пользователя");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Произошла внутренняя ошибка сервера");
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await userCongratulationController.DeleteConfirmed(id);
                return Ok();
            }
            catch (Exception ex)
            {
                // Обработка различных ошибок и возврат подходящего статус-кода
                if (ex is ArgumentException)
                {
                    return BadRequest(ex.Message);
                }
                else if (ex is UnauthorizedAccessException)
                {
                    return Unauthorized();
                }
                else
                {
                    // Логирование ошибки
                    //_logger.LogError(ex, "Ошибка при создании пользователя");
                    return StatusCode(StatusCodes.Status500InternalServerError, "Произошла внутренняя ошибка сервера");
                }
            }
        }
    }
}
