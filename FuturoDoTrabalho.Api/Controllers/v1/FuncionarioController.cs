using Microsoft.AspNetCore.Mvc;
using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Services;

namespace FuturoDoTrabalho.Api.Controllers.v1
{
    // ====================================================================================
    // CONTROLLER: FUNCIONARIO CONTROLLER V1
    // ====================================================================================
    // Controller da versão 1 da API para gerenciamento de funcionários.
    // Versão básica: oferece operações CRUD completas (GET, POST, PUT, DELETE).
    // Não possui paginação nem suporte a PATCH (atualização parcial).
    // ====================================================================================
    [ApiController]
    [Route("api/v{version:apiVersion}/funcionario")]
    [ApiVersion("1.0")]
    public class FuncionarioController : ControllerBase
    {
        private readonly IFuncionarioService _funcionarioService;
        private readonly ILogger<FuncionarioController> _logger;

        public FuncionarioController(
            IFuncionarioService funcionarioService,
            ILogger<FuncionarioController> logger)
        {
            _funcionarioService = funcionarioService;
            _logger = logger;
        }

        /// <summary>
        /// Listar todos os funcionários
        /// </summary>
        /// <returns>Array de funcionários</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<FuncionarioReadDto>>> GetAll()
        {
            _logger.LogInformation("Listando todos os funcionários (v1)");
            var funcionarios = await _funcionarioService.GetAllAsync();
            return Ok(funcionarios);
        }

        /// <summary>
        /// Obter funcionário por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FuncionarioReadDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando funcionário ID {Id}", id);
            var funcionario = await _funcionarioService.GetByIdAsync(id);
            if (funcionario == null)
                return NotFound(new { message = "Funcionário não encontrado" });

            return Ok(funcionario);
        }

        /// <summary>
        /// Criar novo funcionário
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FuncionarioReadDto>> Create([FromBody] FuncionarioCreateDto dto)
        {
            _logger.LogInformation("Criando novo funcionário: {Nome}", dto.Nome);

            try
            {
                var funcionario = await _funcionarioService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = funcionario.Id }, funcionario);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Erro ao criar funcionário: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Atualizar funcionário (completo)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FuncionarioReadDto>> Update(int id, [FromBody] FuncionarioUpdateDto dto)
        {
            _logger.LogInformation("Atualizando funcionário ID {Id}", id);

            try
            {
                var funcionario = await _funcionarioService.UpdateAsync(id, dto);
                if (funcionario == null)
                    return NotFound(new { message = "Funcionário não encontrado" });

                return Ok(funcionario);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Erro ao atualizar funcionário: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletar funcionário
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deletando funcionário ID {Id}", id);
            var resultado = await _funcionarioService.DeleteAsync(id);
            if (!resultado)
                return NotFound(new { message = "Funcionário não encontrado" });

            return NoContent();
        }
    }
}
