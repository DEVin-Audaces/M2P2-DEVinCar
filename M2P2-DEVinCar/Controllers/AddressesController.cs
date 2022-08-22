using M2P2_DEVinCar.Context;
using M2P2_DEVinCar.Dtos;
using M2P2_DEVinCar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M2P2_DEVinCar.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private DEVInCarContext _context;
        private readonly ILogger<AddressesController> _logger;

        public AddressesController(DEVInCarContext context, ILogger<AddressesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Atualiza informações de Endereço no banco de dados
        /// </summary>
        /// <param name="addressId">ID do Endereço</param>
        /// <param name="addressDto">DTO de Endereço com campos Cep, Complemento, Número e Rua opcionais</param>
        /// <returns>Retorna Endereço atualizado com sucesso no banco de dados</returns>
        /// <response code="400">Ao menos um campo deve ser atualizado</response>
        /// <response code="404">ID do Endereço inválido</response>
        /// <response code="204">Endereço atualizado com sucesso</response>
        /// <response code="500">Ocorreu erro durante a execução</response>
        [HttpPatch("{addressId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Patch(int addressId, [FromBody] UpdateAddressDto addressDto)
        {
            try
            {
                bool updateCep = addressDto.Cep != null;
                bool updateComplement = addressDto.Complement != null;
                bool updateNumber = addressDto.Number != null;
                bool updateStreet = addressDto.Street != null;

                bool validPatchRequest = updateCep || updateComplement || updateNumber || updateStreet;

                if (validPatchRequest == false)
                    return BadRequest();

                Address? address = await _context.Addresses.FirstOrDefaultAsync(address => address.Id == addressId);

                if (address == null)
                    return NotFound();

                address.Cep = updateCep ? addressDto.Cep! : address.Cep;
                address.Complement = updateComplement ? addressDto.Complement! : address.Complement;
                address.Number = updateNumber ? (int)addressDto.Number! : address.Number;
                address.Street = updateStreet ? addressDto.Street! : address.Street;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller:{nameof(UsersController)} - Método:{nameof(Patch)}");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Controller:{nameof(UsersController)} - Método:{nameof(Patch)}");
                return StatusCode(500);
            }
        }

        //// GET: api/<AddressesController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<AddressesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<AddressesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<AddressesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AddressesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
