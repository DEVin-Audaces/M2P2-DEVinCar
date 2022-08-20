using M2P2_DEVinCar.Context;
using M2P2_DEVinCar.Dtos;
using M2P2_DEVinCar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.Dtos;

namespace M2P2_DEVinCar.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private DEVInCarContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(DEVInCarContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*[HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        [HttpGet("{id}")]
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

        /// <summary>
        /// Adiciona uma Venda no banco de dados.
        /// </summary>
        /// <param name="userId">ID do usuário(BuyerId).</param>
        /// <param name="CreateBuyDto">DTO de buyer com SellerId e SaleDate.</param>
        /// <returns>Retorna Venda inserida com sucesso no banco de dados.</returns>
        /// <response code="201">Venda inserida com sucesso.</response>
        /// <response code="404">Inserção não realizada ou SellerId/BuyerId não encontrado no banco de dados.</response>
        /// <response code="500">Ocorreu exceção durante a inserção.</response>
        [HttpPost("{userId}/buy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostBuy(int userId, [FromBody] CreateBuyDto CreateBuyDto)
        {
            try
            {
                var userIdSearch = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (userIdSearch is null)
                    return StatusCode(404);

                userIdSearch = await _context.Users.FirstOrDefaultAsync(x => x.Id == CreateBuyDto.SellerId);

                if (userIdSearch is null)
                    return StatusCode(404);

                Sale sale = new();

                sale.SellerId = CreateBuyDto.SellerId;
                sale.BuyerId = userId;
                sale.SaleDate = CreateBuyDto.SaleDate == null ? DateTime.Now : CreateBuyDto.SaleDate;

                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller:{nameof(UsersController)}-Method:{nameof(PostBuy)}");

                return StatusCode(201, $"{sale.Id}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(UsersController)}-Method:{nameof(PostBuy)}");
                return StatusCode(500);
            }
        }
        
        /// <summary>
        /// Insert vendas.
        /// </summary>
        /// <param name="userId">ID do usuário.</param>
        /// <param name="createSaleDTO">DTO de sales com BuyerId e SaleDate.</param>
        /// <returns>Retorna venda inserida com sucesso no banco de dados .</returns>
        /// <response code="201">venda inserida com sucesso.</response>
        /// <response code="404">Inserção não realizada ou SellerId/BuyerId não encontrado no banco de dados.</response>
        /// <response code="500">Ocorreu exceção durante a inserção.</response>
        [HttpPost("{userId}/sales")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostSales(int userId, [FromBody] CreateSaleDto createSaleDTO)
        {
            try
            {
                var userIdSearch = await  _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            
                if (userIdSearch is null)
                    return StatusCode(404);    

                userIdSearch = await  _context.Users.FirstOrDefaultAsync(x => x.Id == createSaleDTO.BuyerId);

                if (userIdSearch is null)
                    return StatusCode(404);    
            
                Sale sale = new();

                sale.BuyerId = createSaleDTO.BuyerId;
                sale.SellerId = userId;
                sale.SaleDate = createSaleDTO.SaleDate == null ? DateTime.Now : createSaleDTO.SaleDate;

            
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Controller:{nameof(UsersController)}-Method:{nameof(PostSales)}");

                return StatusCode(201, $"{sale.Id}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(UsersController)}-Method:{nameof(PostSales)}");
                return StatusCode(500);
            }
            
            

        }

        
        
    }
}