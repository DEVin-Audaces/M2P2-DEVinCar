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

        /// <summary>
        /// Remover Endereço
        /// </summary>
        /// <param name="id">Id Endereço</param>
        /// <returns>Endereço excluído com sucesso do banco de dados</returns>
        /// <response code="204">Endereço excluído com sucesso</response>
        /// <response code="400">Exclusão não realizada pois endereço está relacionado a uma entrega</response>
        /// <response code="404">Exclusão não realizada</response>
        /// <response code="500">Ocorreu erro durante a execução</response>
        [HttpDelete("{addressId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int addressId)
        {
            try
            {
                var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == addressId);

                if (address is null)
                    return NotFound();
                
                bool isAddressInDelivery = true;

                if (isAddressInDelivery)
                    return BadRequest();

                _context.Addresses.Remove(address);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller: {nameof(AddressesController)} - Método {nameof(Delete)}");

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller: {nameof(AddressesController)} - Método {nameof(Delete)}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retorna lista de Endereços
        /// </summary>
        /// <param name="cityId">Id da Cidade</param>
        /// <param name="stateId">Id do Estado</param>
        /// <param name="street">Nome da rua</param>
        /// <param name="cep">Cep do endereço</param>
        /// <returns>Retorna lista de Endereços a partir do(s) parametro(s) especificado</returns>
        /// <response code="200">Endereço(s) encontrado(s) com sucesso</response>
        /// <response code="204">Endereço(s) não encontrado(s)</response>
        /// <response code="500">Ocorreu erro durante a execução</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] int? cityId, int? stateId, string? street, string? cep)
        {
            try
            {
                IEnumerable<Address>? allAddresses = await _context.Addresses
                    .Include(address => address.City)
                    .Include(address => address.City.State)
                    .ToListAsync();

                

                if(cityId is not null)
                   allAddresses = allAddresses.Where(address => address.CityId == cityId).ToList();
                  
                if(stateId is not null)
                   allAddresses = allAddresses.Where(address => address.City.StateId == stateId).ToList();

                if(street is not null)
                   allAddresses = allAddresses.Where(address => address.Street == street).ToList();

                if(cep is not null)
                   allAddresses = allAddresses.Where(address => address.Cep == cep).ToList();


                _logger.LogInformation($"Controller: {nameof(AddressesController)} - Método {nameof(Get)}");

                return allAddresses.Any() ? Ok(allAddresses) : NoContent();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Controller: {nameof(AddressesController)} - Método {nameof(Get)}");

                return StatusCode(500);
            }
        }


    }
}
