using M2P2_DEVinCar.Context;
using M2P2_DEVinCar.Dtos;
using M2P2_DEVinCar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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

                return CreatedAtAction(nameof(PostSales), new { Id = sale.Id }, sale.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(UsersController)}-Method:{nameof(PostSales)}");
                return StatusCode(500);
            }
            
            

        }
        
        /// <summary>
        /// Retorna uma lista de Vendas por SellerId.
        /// </summary>
        /// <param name="userId">ID do usuário(SelleId).</param>
        /// <returns>Retorna um enumerador de Vendas cadastradas no banco de dados.</returns>
        /// <response code="200">Retorno uma lista de Vendas.</response>
        /// <response code="204">Não encontrou uma lista de Vendas por SelleId.</response>
        /// <response code="500">Ocorreu exceção durante a consulta.</response>
        [HttpGet("{userId}/sales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales(int userId)
        {
            try
            {
                var sales = await _context.Sales.Where(x => x.SellerId == userId )
                    .Include(x => x.Buyer)
                    .Include(x => x.Seller)
                    .ToListAsync();
                
                _logger.LogInformation($"Controller:{nameof(UsersController)}-Method:{nameof(GetSales)}");

                return sales.Any() ? Ok(sales) : StatusCode(204);
                

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(UsersController)}-Method:{nameof(GetSales)}");
                return StatusCode(500);
            }
        }



    }
}