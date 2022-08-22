using M2P2_DEVinCar.Context;
using Microsoft.AspNetCore.Mvc;
using M2P2_DEVinCar.Models;


namespace M2P2_DEVinCar.Controllers
{
    [Route("api/car")]
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
        */

        /*
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
