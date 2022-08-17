using M2P2_DEVinCar.Context;
using Microsoft.AspNetCore.Mvc;


namespace M2P2_DEVinCar.Controllers
{
    [Route("api/Car")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private DEVInCarContext _context;

        public CarsController(DEVInCarContext context)
        {
            _context = context;
        }

        /*[HttpGet]
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
        }*/
    }
}
