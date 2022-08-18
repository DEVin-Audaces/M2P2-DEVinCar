using M2P2_DEVinCar.Context;
using Microsoft.AspNetCore.Mvc;


namespace M2P2_DEVinCar.Controllers {
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase {
        private DEVInCarContext _context;

        public UsersController(DEVInCarContext context) {
            _context = context;
        }

        /*[HttpGet]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Retorna usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Retorna o usuário cadastrado no banco de dados</returns>
        /// <response code="200">Retorna usuário</response>
        /// <response code="404">Não encontrou o usuário pesquisado</response>
        /// <response code="500">Ocorreu erro durante a execução</response>
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
