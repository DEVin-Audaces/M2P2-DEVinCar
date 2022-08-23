using M2P2_DEVinCar.Context;
using Microsoft.AspNetCore.Mvc;
using M2P2_DEVinCar.Models;
using Microsoft.EntityFrameworkCore;

namespace M2P2_DEVinCar.Controllers {
    [Route("api/car")]
    [ApiController]
    public class CarsController : ControllerBase {
        private readonly ILogger<UsersController> _logger;
        private DEVInCarContext _context;

        public CarsController(DEVInCarContext context, ILogger<UsersController> logger) {
            _context = context;
            _logger = logger;
        }

        /*[HttpGet]
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }
        */

        /*
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
        */

        /// <summary>
        /// Inserir um carro
        /// </summary>
        /// <param name="car"></param>
        /// <returns>Retorna carro inserido com sucesso no banco de dados</returns>
        /// <response code="201">Carro inserido com sucesso</response>
        /// <response code="400">Inserção nao realizada</response>
        /// <response code="500">Ocorreu um erro durante a execução</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] Car car) {

            try {

                bool carExist = _context.Cars.Any(x => x.Name == car.Name);
                if (carExist) {
                    return StatusCode(400);
                }

                bool priceValid = car.SuggestedPrice > 0;
                if (!priceValid) {
                    return StatusCode(400);
                }


                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return CreatedAtAction("get", new { id = car.Id }, car);

            }
            catch {
                return StatusCode(500);
            }

        }


        /// <summary>
        /// Inserir aluno
        /// </summary>
        /// <param name="car"></param>
        /// <returns>Retorna carro inserido</returns>
        /// <response code = "204">Carro deletado</response>
        /// <response code = "404">Deleção não realizada</response>
        /// /// <response code = "400">Servidor não pode continuar</response>
        /// <response code = "500">Erro execução</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int carid) {
            try {
                var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carid);
                var salecar = await _context.SaleCars.FirstOrDefaultAsync(x => x.Id == carid);
                if (car is null) {
                    _logger.LogInformation($"Controller: {nameof(CarsController)} - Method: {nameof(Delete)} - ID: {carid}");
                    return NotFound();
                }
                if(salecar is not null) {
                    _logger.LogInformation($"Controller: {nameof(CarsController)} - Method: {nameof(Delete)} - ID: {carid}");
                    return BadRequest();
                }

                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Controller: {nameof(CarsController)} - Method: {nameof(Delete)} - ID: {carid}");
                return StatusCode(204);
            }
            catch (Exception e) {
                _logger.LogError(e, $"Controller: {nameof(CarsController)} - Method: {nameof(Delete)} - ID: {carid}");
                return StatusCode(500);
            }
        }
    }
}
