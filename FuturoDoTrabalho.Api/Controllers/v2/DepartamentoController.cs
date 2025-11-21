using Microsoft.AspNetCore.Mvc;
using FuturoDoTrabalho.Api.DTOs;
using FuturoDoTrabalho.Api.Services;

namespace FuturoDoTrabalho.Api.Controllers.v2
{
    // ====================================================================================
    // CONTROLLER: DEPARTAMENTO CONTROLLER V2
    // ====================================================================================
    // Controller da versão 2 da API para gerenciamento de departamentos.
    // Versão avançada: inclui todas as funcionalidades da v1 mais:
    // - Paginação nas listagens (GET com pageNumber e pageSize)
    // - Atualização parcial via PATCH
    // ====================================================================================
    [ApiController]
    [ApiVersion("2.0")]
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
        /// Obtém departamentos com paginação
        /// </summary>
        /// <param name="pageNumber">Número da página (padrão: 1)</param>
        /// <param name="pageSize">Itens por página (padrão: 10, máximo: 100)</param>
        /// <returns>Lista paginada de departamentos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Listando departamentos com paginação - Página {PageNumber}, Tamanho {PageSize}", pageNumber, pageSize);
                
                if (pageSize > 100)
                    pageSize = 100;

                var (data, totalCount, pageCount) = await _service.GetPagedAsync(pageNumber, pageSize);

                return Ok(new
                {
                    data = data,
                    pageNumber = pageNumber,
                    pageSize = pageSize,
                    totalCount = totalCount,
                    totalPages = pageCount
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao listar departamentos");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar departamentos com paginação");
                return StatusCode(500, new { message = "Erro ao listar departamentos", error = ex.Message });
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
                _logger.LogInformation("Buscando departamento ID {DepartamentoId} (v2)", id);
                
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
                _logger.LogInformation("Atualizando departamento ID {DepartamentoId} (v2)", id);
                
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
        /// Atualiza parcialmente um departamento (PATCH)
        /// </summary>
        /// <param name="id">ID do departamento</param>
        /// <param name="dto">Campos a atualizar</param>
        /// <returns>Departamento atualizado</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(DepartamentoReadDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DepartamentoReadDto>> Patch(int id, [FromBody] DepartamentoPatchDto dto)
        {
            try
            {
                _logger.LogInformation("Atualizando parcialmente departamento ID {DepartamentoId} (PATCH v2)", id);
                
                if (id <= 0)
                    return BadRequest(new { message = "ID deve ser maior que zero" });

                var departamento = await _service.PatchAsync(id, dto);
                if (departamento == null)
                    return NotFound(new { message = "Departamento não encontrado" });

                return Ok(departamento);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Erro ao fazer patch em departamento");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer patch em departamento ID {DepartamentoId}", id);
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
                _logger.LogInformation("Deletando departamento ID {DepartamentoId} (v2)", id);
                
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
