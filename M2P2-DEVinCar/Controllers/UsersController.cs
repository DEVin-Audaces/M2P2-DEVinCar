using M2P2_DEVinCar.Context;
using M2P2_DEVinCar.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M2P2_DEVinCar.Controllers {
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly ILogger<UsersController> _logger;
        private DEVInCarContext _context;

        public UsersController(DEVInCarContext context, ILogger<UsersController> logger) {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
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
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                var newUserDto = new UserDto() {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    BirthDate = user.BirthDate
                };

                _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(Get)} - Id: {id}");
                return newUserDto is not null ? Ok(newUserDto) : StatusCode(404);
            }
            catch (Exception e) {
                _logger.LogError(e, $"Controller: {nameof(UsersController)} - Método: {nameof(Get)} - Id: {id}");
                return StatusCode(500);
            }
        }


        [HttpPost]
        public void Post([FromBody] string value) {
        }


        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
