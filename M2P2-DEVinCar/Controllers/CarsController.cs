using M2P2_DEVinCar.Context;
using Microsoft.AspNetCore.Mvc;


namespace M2P2_DEVinCar.Controllers {
    [Route("api/car")]
    [ApiController]
    public class CarsController : ControllerBase {
        private DEVInCarContext _context;

        public CarsController(DEVInCarContext context) {
            _context = context;
        }

        /*[HttpGet]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            try {
                var user = _context.Users.
                    Where(x => x.Id == id).
                    Select(x => new {
                        x.Id,
                        x.Name,
                        x.BirthDate,
                        x.Email,
                    });
                return user is not null ? Ok(user) : StatusCode(404);
            }
            catch (Exception e) {
                return StatusCode(500);
            }
        }


        [HttpPost]
        public void Post([FromBody] string value) {
        }


        [HttpDelete("{id}")]
        public void Delete(int id) {
        }*/
    }
}
