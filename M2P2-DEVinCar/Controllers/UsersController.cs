using M2P2_DEVinCar.Context;
using M2P2_DEVinCar.Dtos;
using M2P2_DEVinCar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace M2P2_DEVinCar.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private DEVInCarContext _context;


        public UsersController(DEVInCarContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retorna os usuários que contém o nome enviado
        /// </summary>
        /// <param name="name"></param>
        /// <param name="birthDateMin"></param>
        /// <param name="birthDateMax"></param>
        /// <returns> Retorna os usuários com data maior que birthDateMin </returns>
        /// <returns> Retorna os usuários com data menor que birthDateMax </returns>
        /// <response code="204"> Caso não encontre resultado, retorna o Status 204 (No Content) </response>
        /// <response code="200"> Caso seja encontrado ao menos um resultado, retorna Status 200 (OK)  </response>
        /// <response code="500"> Em caso de erro na recuperação dos dados, retorna Status 500 </response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string? name = null, DateTime? birthDateMin = null, DateTime? birthDateMax = null)
        {
            try
            {
                List<User> users;

                List<UserDto> usersDto = new();

                List<UserDto> TransformDataToDto(List<User> users)
                {
                    List<UserDto> usersDtos = new();
                    users.ForEach(user =>
                    {

                        var newUser = new UserDto()
                        {
                            Id = user.Id,
                            Name = user.Name,
                            Email = user.Email,
                            BirthDate = user.BirthDate
                        };

                        usersDtos.Add(newUser);
                    });

                    return usersDtos;
                }

                if (birthDateMin != null && birthDateMax != null && name != null)
                {
                    users = await _context.Users
                        .Where(x => x.BirthDate > birthDateMin && x.BirthDate < birthDateMax && x.Name == name)
                        .ToListAsync();
                    usersDto = TransformDataToDto(users);
                    _logger.LogInformation($"Controller:{nameof(UsersController)}-Method:{nameof(Get)}");
                    if (usersDto != null) return Ok(usersDto);
                    else return StatusCode(204);
                }

                if (name != null)
                {
                    users = await _context.Users.Where(x => x.Name == name).ToListAsync();
                    usersDto = TransformDataToDto(users);
                    _logger.LogInformation($"Controller:{nameof(UsersController)}-Method:{nameof(Get)}");
                    if (usersDto != null) return Ok(usersDto);
                    else return StatusCode(204);
                }

                if (birthDateMin != null)
                {
                    users = await _context.Users.Where(x => x.BirthDate > birthDateMin).ToListAsync();
                    usersDto = TransformDataToDto(users);
                    _logger.LogInformation($"Controller:{nameof(UsersController)}-Method:{nameof(Get)}");
                    if (usersDto != null) return Ok(usersDto);
                    else return StatusCode(204);
                }

                if (birthDateMax != null)
                {
                    users = await _context.Users.Where(x => x.BirthDate < birthDateMax).ToListAsync();
                    usersDto = TransformDataToDto(users);
                    _logger.LogInformation($"Controller:{nameof(UsersController)}-Method:{nameof(Get)}");
                    if (usersDto != null) return Ok(usersDto);
                    else return StatusCode(204);
                }

                users = await _context.Users.ToListAsync();
                usersDto = TransformDataToDto(users);
                _logger.LogInformation($"Controller:{nameof(UsersController)}-Method:{nameof(Get)}");
                if (usersDto != null) return Ok(usersDto);
                else return StatusCode(204);
            }

            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(UsersController)}-Method:{nameof(Get)}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retorna usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Retorna o usuário cadastrado no banco de dados</returns>
        /// <response code="200">Retorna usuário</response>
        /// <response code="404">Não encontrou o usuário pesquisado</response>
        /// <response code="500">Ocorreu erro durante a execução</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                var newUserDto = new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    BirthDate = user.BirthDate
                };

                _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(Get)} - Id: {id}");
                return newUserDto is not null ? Ok(newUserDto) : StatusCode(404);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller: {nameof(UsersController)} - Método: {nameof(Get)} - Id: {id}");
                return StatusCode(500);
            }
        }


        /// <summary>
        /// Inserir usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Retorna Id do usuário inserido</returns>
        /// <response code = "201">Usuário inserido com sucesso</response>
        /// <response code = "400">Inserção não realizada</response>
        /// <response code = "500">Erro execução</response>
        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] User user)
        {
            try
            {
                bool emailsExists = _context.Users.Any(x => x.Email == user.Email);
                if (emailsExists)
                    return BadRequest();

                bool emailIsRight = new EmailAddressAttribute().IsValid(user.Email);
                if (!emailIsRight)
                    return BadRequest();

                bool ValidatesPassword = Regex.IsMatch(user.Password, @"^(\w)\1*$");
                if (ValidatesPassword)
                    return BadRequest();


                if (Convert.ToDateTime(user.BirthDate).AddYears(18) > DateTime.Now)
                    return BadRequest();


                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return StatusCode(201, $"{user.Id}"); ;
            }
            catch
            {
                return StatusCode(500);
            }

        }

        /*
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
                var userIdSearch = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (userIdSearch is null)
                    return StatusCode(404);

                userIdSearch = await _context.Users.FirstOrDefaultAsync(x => x.Id == createSaleDTO.BuyerId);

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
                var sales = await _context.Sales.Where(x => x.SellerId == userId)
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

        /// <summary>
        /// Retorna uma lista de Vendas por BuyerId.
        /// </summary>
        /// <param name="userId">ID do usuário(BuyerId).</param>
        /// <returns>Retorna uma lista de Vendas cadastradas no banco de dados.</returns>
        /// <response code="200">Retorna uma lista de Vendas de um determinado BuyerId.</response>
        /// <response code="204">Não encontrou uma lista de Vendas de um determinado BuyerId.</response>
        /// <response code="500">Ocorreu exceção durante a consulta.</response>
        [HttpGet("{userId}/buy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Sale>>> GetBuyer(int userId)
        {
            try
            {
                var sales = await _context.Sales.Where(x => x.BuyerId == userId)
                    .Include(x => x.Buyer)
                    .Include(x => x.Seller)
                    .ToListAsync();

                _logger.LogInformation($"Controller:{nameof(UsersController)}-Method:{nameof(GetBuyer)}");

                return sales.Any() ? Ok(sales) : StatusCode(204);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(UsersController)}-Method:{nameof(GetBuyer)}");
                return StatusCode(500);
            }
        }
    }
}