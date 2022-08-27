using M2P2_DEVinCar.Context;
using M2P2_DEVinCar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace M2P2_DEVinCar.Controllers
{
    [Route("api/deliver")]
    [ApiController]
    public class DeliverController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private DEVInCarContext _context;


        public DeliverController(DEVInCarContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retorna uma lista de entregas
        /// </summary>
        /// <param name="addressId">Id do endereço </param>
        /// <param name="saleId">Id da venda</param>
        /// <returns>Retorna uma lista de entregas cadastradas no banco de dados</returns>
        /// <response code="200">Retorna lista</response>
        /// <response code="204">Não encontrou a entrega através dos Ids informados</response>
        /// <response code="500">Ocorreu erro durante a execução</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDeliver([FromQuery] int? addressId, [FromQuery] int? saleId)
        {
            try
            {
                if (addressId == null && saleId == null)
                {
                    return await _context.Deliveries.ToListAsync();
                }

                var deliveries = await _context.Deliveries.Where(x=> x.AddressId == addressId && x.SaleId==saleId).ToListAsync();

                _logger.LogInformation($"Controller:{nameof(DeliverController)}-Method:{nameof(GetDeliver)}");

                return deliveries.Any()? Ok(deliveries):StatusCode(204);


            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(DeliverController)}-Method:{nameof(GetDeliver)}");
                return StatusCode(500);
            }

        }

    }
}
