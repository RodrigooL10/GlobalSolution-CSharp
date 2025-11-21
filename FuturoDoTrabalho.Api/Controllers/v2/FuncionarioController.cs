using Microsoft.AspNetCore.Mvc;
using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Services;

namespace FuturoDoTrabalho.Api.Controllers.v2
{
    // ====================================================================================
    // CONTROLLER: FUNCIONARIO CONTROLLER V2
    // ====================================================================================
    // Controller da versão 2 da API para gerenciamento de funcionários.
    // Versão avançada: inclui todas as funcionalidades da v1 mais:
    // - Paginação nas listagens (GET com pageNumber e pageSize)
    // - Atualização parcial via PATCH
    // - Filtros avançados (por status ativo/inativo)
    // ====================================================================================
    [ApiController]
    [Route("api/v{version:apiVersion}/funcionario")]
    [ApiVersion("2.0")]
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
        /// Listar funcionários com paginação (v2)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool? ativo = null)
        {
            _logger.LogInformation("Listando funcionários com paginação - Página {PageNumber}, Tamanho {PageSize}", pageNumber, pageSize);

            // Validar e corrigir parâmetros de paginação
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10; // Limitar máximo de 100 itens por página

            // Buscar dados paginados do service
            var (data, totalCount, pageCount) = await _funcionarioService.GetPagedAsync(pageNumber, pageSize);

            // Aplicar filtro por status ativo/inativo se especificado
            if (ativo.HasValue)
            {
                data = data.Where(f => f.Ativo == ativo.Value).ToList();
            }

            // Retornar resposta paginada com metadados
            return Ok(new
            {
                data,
                pageNumber,
                pageSize,
                totalCount,
                totalPages = pageCount
            });
        }

        /// <summary>
        /// Obter funcionário por ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FuncionarioReadDto>> GetById(int id)
        {
            _logger.LogInformation("Buscando funcionário ID {Id} (v2)", id);
            var funcionario = await _funcionarioService.GetByIdAsync(id);
            if (funcionario == null)
                return NotFound(new { message = "Funcionário não encontrado" });

            return Ok(funcionario);
        }

        /// <summary>
        /// Criar novo funcionário (v2)
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FuncionarioReadDto>> Create([FromBody] FuncionarioCreateDto dto)
        {
            _logger.LogInformation("Criando novo funcionário: {Nome} (v2)", dto.Nome);

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
        /// Atualizar funcionário completo (PUT)
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FuncionarioReadDto>> Update(int id, [FromBody] FuncionarioUpdateDto dto)
        {
            _logger.LogInformation("Atualizando funcionário ID {Id} (v2)", id);

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
        /// Atualizar parcialmente funcionário (PATCH) - APENAS v2
        /// </summary>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FuncionarioReadDto>> Patch(int id, [FromBody] FuncionarioPatchDto dto)
        {
            _logger.LogInformation("Atualizando parcialmente funcionário ID {Id} (PATCH v2)", id);

            try
            {
                var funcionario = await _funcionarioService.PatchAsync(id, dto);
                if (funcionario == null)
                    return NotFound(new { message = "Funcionário não encontrado" });

                return Ok(funcionario);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Erro ao fazer PATCH: {Mensagem}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletar funcionário (v2)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deletando funcionário ID {Id} (v2)", id);
            var resultado = await _funcionarioService.DeleteAsync(id);
            if (!resultado)
                return NotFound(new { message = "Funcionário não encontrado" });

            return NoContent();
        }
    }
}
