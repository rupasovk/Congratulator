using CongratulatorWebAppAPI.BuisnesObjects;
using CongratulatorWebAppAPI.BuisnesObjects.DtoObjects;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CongratulatorWebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        UsersController usersController = new UsersController();
        UserImageController usersImageController = new UserImageController();

        // GET: api/<UserAPIController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var result = usersController?.GetUserList();
            return result?.Select(s => JsonSerializer.Serialize(s)) ?? new List<string>() { };
        }

        // GET api/<UserAPIController>/5
        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            return usersController?.GetUserDetails(id)?.ToString() ?? "";
        }

        // POST api/<UserAPIController>
        [EnableCors("CorsPolicy")]
        [HttpPost]
        //public async Task<IActionResult> Post([FromBody] UserDto userDto)
        public async Task<IActionResult> Post([FromForm] UserDto UserDto, [FromForm] IFormFile UserImage)
        {
            try
            {
                var user = new User(UserDto);

                await usersController.CreateAsync(user);

                // Create the user image
                if (UserImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await UserImage.CopyToAsync(memoryStream);
                        var userImage = new UserImage(memoryStream.ToArray(), user);
                        await usersImageController.CreateAsync(userImage, user);
                    }
                }

                //var userImage = new UserImage(Convert.FromBase64String(userDto.UserImage), user);
                //await usersImageController.CreateAsync(userImage, user);

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
        public async Task<IActionResult> Put(Guid id, string _name, string _surname, DateTime _birthDay, string _email, string country)
        {
            try
            {
                var user = new User(id, _name, _surname, _birthDay, _email, country);
                await usersController.UpdateAsync(id, user);
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

        // DELETE api/<UserAPIController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await usersController.DeleteConfirmed(id);
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
