using M2P2_DEVinCar.Context;
using M2P2_DEVinCar.Dtos;
using M2P2_DEVinCar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M2P2_DEVinCar.Controllers
{
    [Route("api/state")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private DEVInCarContext _context;
        private readonly ILogger<StatesController> _logger;

        public StatesController(DEVInCarContext context, ILogger<StatesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Localiza um estado no banco de dados.
        /// </summary>
        /// <param name="stateId">ID do estado</param
        /// <returns>Retorna um estado localizado pelo ID no banco de dados</returns>
        /// <response code="200">Retorna o estado com o ID pesquisado</response>
        /// <response code="404">ID de estado inválido</response>
        /// <response code="500">Ocorreu exceção durante a operação</response>
        [HttpGet("{stateId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<State>> GetState(int stateId)
        {
            try
            {
                var stateResult = await _context.States.FirstOrDefaultAsync(state => state.Id == stateId);

                if (stateResult == null)
                    return NotFound();

                _logger.LogInformation($"Controller:{nameof(StatesController)}-Method:-{nameof(GetState)}");

                return Ok(stateResult);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Controller:{nameof(StatesController)}-Method:-{nameof(GetState)}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Adiciona uma Cidade no banco de dados.
        /// </summary>
        /// <param name="stateId">ID do estado</param>
        /// <param name="CreateCityDto">DTO de cidade contendo apenas o nome</param>
        /// <returns>Retorna Cidade inserida com sucesso no banco de dados</returns>
        /// <response code="201">Cidade inserida com sucesso</response>
        /// <response code="400">Esta cidade já está registrada no banco de dados</response>
        /// <response code="404">ID de estado inválido</response>
        /// <response code="500">Ocorreu exceção durante a operação</response>
        [HttpPost("{stateId}/city")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostCity(int stateId, [FromBody] CreateCityDto cityDto)
        {
            try
            {
                bool stateIsValid = await _context.States
                    .AnyAsync(state => state.Id == stateId);

                if (stateIsValid == false)
                    return NotFound();

                bool cityAlreadyExists = await _context.Cities
                    .AnyAsync(city => city.Name == cityDto.Name
                    && city.StateId == stateId);

                if (cityAlreadyExists == true)
                    return BadRequest();

                City city = new City()
                {
                    StateId = stateId,
                    Name = cityDto.Name
                };

                _context.Cities.Add(city);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller:{nameof(StatesController)}-Method:{nameof(PostCity)}");

                return StatusCode(201, new { Id = city.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Controller:{nameof(StatesController)}-Method:{nameof(PostCity)}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Adiciona um Endereço no banco de dados
        /// </summary>
        /// <param name="stateId">Id do Estado</param>
        /// <param name="cityId">Id da Cidade</param>
        /// <param name="addressDto">Dto do Endereço contento Id da cidade, rua, numero, cep e complemento(opcional)</param>
        /// <returns>Retorna Endereço inserido com sucesso no banco de dados</returns>
        /// <response code="201">Endereço criado com sucesso</response>
        /// <response code="400">Cidade contém ID de estado inválido </response>
        /// <response code="404">ID de cidade ou estado inválidos</response>
        /// <response code="500">Ocorreu exceção durante a operação</response>
        [HttpPost("{stateId}/city/{cityId}/address")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAddress(int stateId, int cityId, [FromBody] CreateAddressDto addressDto)
        {
            try
            {
                bool stateIsValid = await _context.States
                    .AnyAsync(state => state.Id == stateId);

                if (stateIsValid == false)
                    return NotFound();

                bool cityIsValid = await _context.Cities
                    .AnyAsync(city => city.Id == cityId);

                if (cityIsValid == false)
                    return NotFound();

                var city = await _context.Cities.FirstOrDefaultAsync(city => city.Id == cityId);
                bool cityAndStateMatch = city.StateId == stateId;

                if (cityAndStateMatch == false)
                    return BadRequest();

                Address address = new()
                {
                    CityId = cityId,
                    Street = addressDto.Street,
                    Number = (int)addressDto.Number,
                    Cep = addressDto.Cep,
                    Complement = addressDto.Complement
                };

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller:{nameof(StatesController)}-Method:{nameof(PostAddress)}");


                return StatusCode(201, new { Id = address.Id });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Controller:{nameof(StatesController)}-Method:{nameof(PostAddress)}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retorna uma lista de Estados
        /// </summary>
        /// <param name="name">Nome de algum estado</param>
        /// <returns>Retorna Estado com o nome solicitado</returns>
        /// <returns>Se não solicitado nome, retorna todos os estados.</returns>
        /// <response code="200">Retorna uma lista de estados</response>
        /// <response code="204">Não encontrou o estado</response>
        /// <response code="500">Ocorreu erro durante a execução</response>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<State>>> GetState([FromQuery]string? name)
        {
            try
            {
                

                if (name != null)
                {
                    var stateName = await _context.States.FirstOrDefaultAsync(nameState => nameState.Name == name);

                    _logger.LogInformation($"Controller: {nameof(StatesController)} = Metodo: {nameof(GetState)}");
                        
                    return stateName is not null ? Ok(stateName) : StatusCode(204);
                }

                var states = await _context.States.ToListAsync();

                _logger.LogInformation($"Controller: {nameof(StatesController)} = Metodo: {nameof(GetState)}");

                return states.Any() ? Ok(states) : StatusCode(204);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Controller: {nameof(StatesController)} = Metodo: {nameof(GetState)}");
                return StatusCode(500);
            }

          
        }

        /// <summary>
        /// Retorna uma lista de Cidades
        /// </summary>
        /// <param name="stateId">Id do estado</param>
        /// <param name="CreateCityDto">DTO de cidade contendo apenas o nome</param>
        /// <returns>Retorna Cidade cadastrada no banco de dados</returns>
        /// <returns>Se não solicitado nome, retorna todas as cidades.</returns>
        /// <response code="200">Retorna uma lista de ciddades</response>
        /// <response code="204">Não encontrou a cidade</response>
        /// <response code="404">Não encontrou o estado</response>
        /// <response code="500">Ocorreu erro durante a execução</response>

        [HttpGet("{stateId}/city")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<State>>> GetCity(int stateId, [FromQuery] string? name)
        {
            try
            {
                bool stateIsValid = await _context.States.AnyAsync(state => state.Id == stateId);
                if (stateIsValid == false)
                    return NotFound();

                var cities = await _context.Cities.Where(x => x.StateId == stateId)
                    .Include(x => x.State)
                    .ToListAsync();

                _logger.LogInformation($"Controller: {nameof(StatesController)} - Método: {nameof(GetCity)}");

                if(name == null)
                {
                    return cities is not null ? Ok(cities) : StatusCode(404);
                }


                var city = cities.FirstOrDefault(y => y.Name == name);


                return city is not null ? Ok(city) : StatusCode(204);


            }
            catch (Exception e)
            {
                _logger.LogInformation($"Controller: {nameof(StatesController)} - Método: {nameof(GetCity)}");
                return StatusCode(500);
            }
        }


        /*[HttpGet("{id}")]
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
