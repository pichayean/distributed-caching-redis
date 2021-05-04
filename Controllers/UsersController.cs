using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace distributed_caching_redis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
	    private readonly IUsersService _usersService;
       
        public UsersController(IUsersService usersService)
	    {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IEnumerable<Users>> Get()
        {
            return await _usersService.GetUsersAsync();
        }
    }
}
