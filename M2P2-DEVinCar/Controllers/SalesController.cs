using M2P2_DEVinCar.Context;
using M2P2_DEVinCar.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Dtos;

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
        public async Task<IActionResult> Patch(int saleId, int carId, decimal unitPrice)
        {
            try
            {
                if (unitPrice <= 0)
                {
                    return StatusCode(400);
                }
                var _sales = await _context.Sales.FirstOrDefaultAsync(x => x.Id == saleId);

                if (_sales is null)
                    return StatusCode(404);

                var _saleCar = await _context.SaleCars.Where(x => x.SaleId == _sales.Id && x.CarId == carId).FirstOrDefaultAsync();

                if (_saleCar is null)
                    return StatusCode(404);

                _saleCar.UnitPrice = unitPrice;
                _context.Entry(_saleCar).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller:{nameof(SalesController)}-Method:{nameof(Patch)}");

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(SalesController)}-Method:{nameof(Patch)}");
                return StatusCode(500);
            }



        }

    }
}