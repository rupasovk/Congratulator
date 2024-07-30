using CongratulatorWebAppAPI.BuisnesObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CongratulatorWebAppAPI.Controllers.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserImageAPIController : ControllerBase
    {
        UserImageController userImageController = new UserImageController();
        UsersController userController = new UsersController();

        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            return userImageController?.GetUserImageById(id)?.ToString() ?? "";
        }

        [EnableCors("CorsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserImage userImage, Guid userId)
        {
            try
            {
                //var userCongratulation = new UserImage() { Message = message, Type = type };

                var user = userController.GetUserDetails(userId);

                await userImageController.CreateAsync(userImage, user);

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
                //await userImageController.UpdateAsync(id, userCongratulation);
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
                await userImageController.DeleteConfirmed(id);
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
