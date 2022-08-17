using M2P2_DEVinCar.Context;
using Microsoft.AspNetCore.Mvc;



namespace M2P2_DEVinCar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
            private DEVInCarContext _context;
            private readonly ILogger<UsersController> _logger;

            public UsersController(DEVInCarContext context
                , ILogger<UsersController> logger)
            {
                _context = context;
                _logger = logger;
            }

            [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
