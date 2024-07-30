using CongratulatorWebAppAPI.BuisnesObjects;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CongratulatorWebAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthdaysAPIController : ControllerBase
    {
        UsersController usersController = new UsersController();
        UserImageController usersImageController = new UserImageController();

        [HttpGet]
        public IEnumerable<string> GetNearBirthdays()
        {
            var result = usersController?.GetNearBirthdaysList(5);
            return result?.Select(s => JsonSerializer.Serialize(s)) ?? new List<string>() { };
        }
    }
}
