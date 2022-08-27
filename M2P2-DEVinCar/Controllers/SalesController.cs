using M2P2_DEVinCar.Dtos;
using M2P2_DEVinCar.Models;
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
        /// Cadastra venda de carro.
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="createSaleCar"></param>
        /// <returns>Retorna venda de carro</returns>
        /// <response code="201">Retorna o Id do saleCar criado.</response>
        /// <response code="400">Atributo CarId não enviado e UnitPrice ou um Amount <= 0.</response>
        /// <response code="404">Atributo CarId ou SaleId não existe.</response>
        /// <response code="500">Erro durante a execução.</response>

        [HttpPost("{saleId}/item")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SaleCar>> PostSaleCar(int saleId, [FromBody] CreateSaleCarDto createSaleCar)
        {
            try
            {
                var saleIdSearch = await _context.Sales.FirstOrDefaultAsync(x => x.Id == saleId);

                if (saleIdSearch is null)
                    return StatusCode(404);

                var carIdSearch = await _context.Cars.FirstOrDefaultAsync(x => x.Id == createSaleCar.CarId);

                if (carIdSearch is null)
                    return StatusCode(404);

                if (createSaleCar.CarId is null)
                    return BadRequest();

                if (createSaleCar.UnitPrice <= 0 || createSaleCar.Amount <= 0)
                    return BadRequest();

                if (createSaleCar.UnitPrice is null)
                {
                    var cars = await _context.Cars.FirstOrDefaultAsync(x => x.Id == createSaleCar.CarId);

                    createSaleCar.UnitPrice = cars.SuggestedPrice;
                }

                if (createSaleCar.Amount is null)
                {
                    createSaleCar.Amount = 1;
                }

                SaleCar saleCar = new();

                saleCar.SaleId = saleId;
                saleCar.CarId = (int)createSaleCar.CarId;
                saleCar.UnitPrice = (decimal)createSaleCar.UnitPrice;
                saleCar.Amount = (int)createSaleCar.Amount;

                _context.SaleCars.Add(saleCar);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller: {nameof(SalesController)} - Method: {nameof(PostSaleCar)}");

                return saleCar is null ? StatusCode(404) : StatusCode(201, $"{saleCar.Id}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller: {nameof(SalesController)} - Method: {nameof(PostSaleCar)}");

                return StatusCode(500);
            }
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