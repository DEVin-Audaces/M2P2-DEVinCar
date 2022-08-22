using M2P2_DEVinCar.Context;
using Microsoft.AspNetCore.Mvc;
using M2P2_DEVinCar.Models;
using Microsoft.EntityFrameworkCore;

namespace M2P2_DEVinCar.Controllers
{
    [Route("api/car")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private DEVInCarContext _context;
        private readonly ILogger<CarsController> _logger;
        public CarsController(DEVInCarContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        */

        /// <summary>
        /// Retorna carro
        /// </summary>
        /// <param name="car"></param>
        /// <returns>Retorna carro cadastrado no banco de dados</returns>
        /// <response code="200">Retorna carro</response>
        /// <response code="404">Não encontrou carro pesquisado</response>
        /// <response code="500">Ocorreu erro durante a execução</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);
                _logger.LogInformation($"Controller: {nameof(CarsController)} - Método: {nameof(Get)} - Id: {id}");
                return car is not null ? Ok(car) : StatusCode(404);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller: {nameof(CarsController)} - Método: {nameof(Get)} - Id: {id}");
                return StatusCode(500);
            }
        }

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
        public async Task<IActionResult> Post([FromBody] Car car)
        {

            try
            {

                bool carExist = _context.Cars.Any(x => x.Name == car.Name);
                if (carExist)
                {
                    return StatusCode(400);
                }

                bool priceValid = car.SuggestedPrice > 0;
                if (!priceValid)
                {
                    return StatusCode(400);
                }


                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return CreatedAtAction("get", new { id = car.Id }, car);

            }
            catch
            {
                return StatusCode(500);
            }

        }

        /*
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
