using M2P2_DEVinCar.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace M2P2_DEVinCar.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private DEVInCarContext _context;
        private readonly ILogger<SalesController> _logger;

        public SalesController(DEVInCarContext context, ILogger<SalesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Atualizar o preço unitário, através do ID da venda e do carro.
        /// </summary>
        /// <param name="saleId">Id da venda.</param>
        /// <param name="carId">Id do carro.</param>
        /// <param name="unitPrice">Preço únitário do veículo da venda.</param>
        /// <response code="204">Preço unitário atualizado com sucesso.</response>
        /// <response code="404">Não encontrado a venda ou o carro.</response>
        /// <response code="400">Preço unitário menor ou igual a zero.</response>
        /// <response code="500">Ocorreu exceção durante a consulta.</response>
        [HttpPatch("{saleId}/car/{carId}/price/{unitPrice}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchPrice(int saleId, int carId, decimal unitPrice)
        {
            try
            {
                if (unitPrice <= 0)
                {
                    return StatusCode(400);
                }
             
                var _saleCar = await _context.SaleCars.FirstOrDefaultAsync(x => x.SaleId == saleId && x.CarId == carId);

                if (_saleCar is null)
                    return StatusCode(404);

                _saleCar.UnitPrice = unitPrice;
                _context.Entry(_saleCar).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller:{nameof(SalesController)}-Method:{nameof(PatchPrice)}");

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(SalesController)}-Method:{nameof(PatchPrice)}");
                return StatusCode(500);
            }



        }

        /// <summary>
        /// Atualizar, através do SaleId e CarId, a quantide de unidades de Carro.
        /// </summary>
        /// <param name="saleId">Id da Venda.</param>
        /// <param name="carId">Id do Carro.</param>
        /// <param name="amount">Quantidade de unidades de Carro comercializadas em uma Venda.</param>
        /// <response code="204">Quantidade de unidades de Carro em uma Venda atualizada com sucesso.</response>
        /// <response code="400">Quantidade menor ou igual a zero.</response>
        /// <response code="404">O saleId ou carId não foi encontrado.</response>
        /// <response code="500">Ocorreu exceção durante a consulta.</response>
        [HttpPatch("{saleId}/car/{carId}/amount/{amount}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> PatchAmount(int saleId, int carId, int amount)
        {
            try
            {
                var saleCar = await _context
                    .SaleCars
                    .FirstOrDefaultAsync(x => x.SaleId == saleId && x.CarId == carId);

                if (saleCar is null)
                    return StatusCode(404);

                if (amount <= 0)
                    return StatusCode(400);

                saleCar.Amount = amount;

                _context.Entry(saleCar).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller:{nameof(SalesController)}-Method:{nameof(PatchAmount)}");

                return StatusCode(204);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(SalesController)}-Method:{nameof(PatchAmount)}");
                return StatusCode(500);
            }
        }

    }
}