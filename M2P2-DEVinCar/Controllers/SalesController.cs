using M2P2_DEVinCar.Dtos;
using M2P2_DEVinCar.Models;
using M2P2_DEVinCar.Context;
using M2P2_DEVinCar.Dtos;
using M2P2_DEVinCar.Models;
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
        /// Retorna vendas por id 
        /// </summary>
        /// <param name="saleId">Id da venda.</param>
        /// <returns>Retorna as informações de venda</returns>
        /// <response code="200">Retorna venda</response>
        /// <response code="404">Não encontrou a venda pesquisada</response>
        /// <response code="500">Ocorreu erro durante a execução</response>
        [HttpGet("{saleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int saleId)
        {
            try
            {
                
                var _sale = await _context.Sales.FirstOrDefaultAsync(x => x.Id == saleId);

                if (_sale is null)
                    return StatusCode(404);
                
                var _userBuyer = await _context.Users.FirstOrDefaultAsync(x => x.Id == _sale.BuyerId);
                var _userSeller = await _context.Users.FirstOrDefaultAsync(x => x.Id == _sale.SellerId);

                var _saleDto = new SalesDto()
                {
                    Id = _sale.Id,
                    NameBuyer = _userBuyer.Name,
                    NameSeller = _userSeller.Name,
                    SaleDate = _sale.SaleDate,
                    listSale = new List<ItemSaleDto>()
                };

                var _salesCar = await _context.SaleCars.Where(x => x.SaleId == _sale.Id).ToListAsync();

                
                foreach (SaleCar saleCar in _salesCar)
                {
                    ItemSaleDto itemSale = new();
                    var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == saleCar.CarId);
                    itemSale.NameProduct = car.Name;
                    itemSale.UnitPrice = saleCar.UnitPrice;
                    itemSale.Amount = saleCar.Amount;
                    itemSale.Total = (saleCar.UnitPrice * saleCar.Amount);
                    _saleDto.listSale.Add(itemSale);
                }
                
                _logger.LogInformation($"Controller: {nameof(SalesController)} - Método: {nameof(Get)} - Id: {saleId}");
                return Ok(_saleDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller: {nameof(SalesController)} - Método: {nameof(Get)} - Id: {saleId}");
                return StatusCode(500);
            }
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
        /// <summary>
        /// Cadastra entrega.
        /// </summary>
        /// <param name="saleId">ID da venda.</param>
        /// <param name="createDeliveryDTO">DTO de delivery com AddressId e DeliveryForecast.</param>
        /// <returns>Retorna entrega inserida com sucesso no banco de dados.</returns>
        /// <response code="201">Entrega inserida com sucesso.</response>
        /// <response code="400">Inserção não realizada.</response>
        /// <response code="404">Inserção não realizada ou AddressId/saleId não encontrado no banco de dados.</response>
        /// <response code="500">Ocorreu exceção durante a inserção.</response>
        [HttpPost("{saleId}/deliver")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostDeliver(int saleId, [FromBody] CreateDeliveryDto createDeliveryDTO)
        {
            try
            {
                var saleIdSearch = await _context.Sales.FirstOrDefaultAsync(x => x.Id == saleId);

                if (saleIdSearch is null)
                    return StatusCode(404);

                var addressIdSearch = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == createDeliveryDTO.AddressId);

                if (addressIdSearch is null)
                    return StatusCode(404);

                if (createDeliveryDTO.AddressId is null)
                    return StatusCode(400);

                if (createDeliveryDTO.DeliveryForecast < DateTime.Now)
                    return StatusCode(400);


                Delivery deliver = new();

                deliver.AddressId = (int)createDeliveryDTO.AddressId;
                deliver.SaleId = saleId;
                deliver.DeliveryForecast = createDeliveryDTO.DeliveryForecast == null ? DateTime.Now.AddDays(7) : createDeliveryDTO.DeliveryForecast;


                _context.Deliveries.Add(deliver);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller:{nameof(SalesController)}-Method:{nameof(PostDeliver)}");

                return StatusCode(201);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(SalesController)}-Method:{nameof(PostDeliver)}");
                return StatusCode(500);
            }
        }
    }
}