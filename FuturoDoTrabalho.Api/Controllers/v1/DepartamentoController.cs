using Microsoft.AspNetCore.Mvc;
using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Services;

namespace FuturoDoTrabalho.Api.Controllers.v1
{
    // ====================================================================================
    // CONTROLLER: DEPARTAMENTO CONTROLLER V1
    // ====================================================================================
    // Controller da versão 1 da API para gerenciamento de departamentos.
    // Versão básica: oferece operações CRUD completas (GET, POST, PUT, DELETE).
    // Não possui paginação nem suporte a PATCH (atualização parcial).
    // ====================================================================================
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoService _service;
        private readonly ILogger<DepartamentoController> _logger;

        public DepartamentoController(IDepartamentoService service, ILogger<DepartamentoController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtém todos os departamentos
        /// </summary>
        /// <returns>Lista de departamentos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DepartamentoReadDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<DepartamentoReadDto>>> GetAll()
        {
            try
            {
                _logger.LogInformation("Listando todos os departamentos (v1)");
                var departamentos = await _service.GetAllAsync();
                return Ok(departamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter departamentos");
                return StatusCode(500, new { message = "Erro ao obter departamentos", error = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um departamento por ID
        /// </summary>
        /// <param name="id">ID do departamento</param>
        /// <returns>Dados do departamento</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DepartamentoReadDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DepartamentoReadDto>> GetById(int id)
        {
            try
            {
                _logger.LogInformation("Buscando departamento ID {DepartamentoId} (v1)", id);
                
                if (id <= 0)
                    return BadRequest(new { message = "ID deve ser maior que zero" });

                var departamento = await _service.GetByIdAsync(id);
                if (departamento == null)
                    return NotFound(new { message = "Departamento não encontrado" });

                return Ok(departamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter departamento ID {DepartamentoId}", id);
                return StatusCode(500, new { message = "Erro ao obter departamento", error = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo departamento
        /// </summary>
        /// <param name="dto">Dados do departamento</param>
        /// <returns>Departamento criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(DepartamentoReadDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DepartamentoReadDto>> Create([FromBody] DepartamentoCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Criando novo departamento: {Nome}", dto.Nome);
                
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var departamento = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = departamento.Id }, departamento);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro ao criar departamento");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar departamento");
                return StatusCode(500, new { message = "Erro ao criar departamento", error = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um departamento existente
        /// </summary>
        /// <param name="id">ID do departamento</param>
        /// <param name="dto">Dados a serem atualizados</param>
        /// <returns>Departamento atualizado</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DepartamentoReadDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DepartamentoReadDto>> Update(int id, [FromBody] DepartamentoUpdateDto dto)
        {
            try
            {
                _logger.LogInformation("Atualizando departamento ID {DepartamentoId} (v1)", id);
                
                if (id <= 0)
                    return BadRequest(new { message = "ID deve ser maior que zero" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var departamento = await _service.UpdateAsync(id, dto);
                if (departamento == null)
                    return NotFound(new { message = "Departamento não encontrado" });

                return Ok(departamento);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro ao atualizar departamento");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar departamento ID {DepartamentoId}", id);
                return StatusCode(500, new { message = "Erro ao atualizar departamento", error = ex.Message });
            }
        }

        /// <summary>
        /// Deleta um departamento
        /// </summary>
        /// <param name="id">ID do departamento</param>
        /// <returns>Confirmação de deleção</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deletando departamento ID {DepartamentoId} (v1)", id);
                
                if (id <= 0)
                    return BadRequest(new { message = "ID deve ser maior que zero" });

                var resultado = await _service.DeleteAsync(id);
                if (!resultado)
                    return NotFound(new { message = "Departamento não encontrado" });

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro ao deletar departamento");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar departamento ID {DepartamentoId}", id);
                return StatusCode(500, new { message = "Erro ao deletar departamento", error = ex.Message });
            }
        }
    }
}
