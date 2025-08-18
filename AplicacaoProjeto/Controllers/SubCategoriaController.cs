using Domain.Dtos.SubCategoriaDtos;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AplicacaoProjeto.Controllers
{
    public class SubCategoriaController : ControllerBase
    {
        private readonly ISubCategoriaService _subCategoriaService;
        private readonly ILogger<SubCategoriaController> _logger;
        public SubCategoriaController(ISubCategoriaService subcategoriaService, ILogger<SubCategoriaController> logger)
        {
            _subCategoriaService = subcategoriaService;
            _logger = logger;
        }

        [HttpPost("CriarSubCategoria")]
        [SwaggerOperation(Summary = "Criar Subcategorias", OperationId = "Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarSubCategoria([FromBody] SubCategoriaDto subCategoria)
        {
            try
            {
                _logger.LogInformation("Iniciando o cadastro da subcategoria: {NomeSubCategoria}", subCategoria);

                SubCategoria categoria = await _subCategoriaService.CriarSubCategoria(subCategoria);

                if (categoria != null)
                {
                    _logger.LogInformation("SubCategoria cadastrada com sucesso. ID: {ID}", categoria.Id);
                    return Created();
                }

                _logger.LogWarning("Falha ao cadastrar Subcategoria. Nome: {NomeSubCategoria}", subCategoria);
                return BadRequest($"Não foi possível cadastrar a subcategoria. SubCategoriaNome: {subCategoria}.");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Erro ao salvar subcategoria: {NomeSubCategoria}", subCategoria);
                return StatusCode(500, new { erro = "Ocorreu um erro ao processar a requisição." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar subcategoria: {NomeSubCategoria}", subCategoria);
                return StatusCode(500, new { erro = "Ocorreu um erro ao processar a requisição." });
            }
        }

        [HttpGet("BuscarSubCategoria")]
        [SwaggerOperation(Summary = "Buscar subcategorias", OperationId = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarSubCategorias(
            [FromQuery] int? id)
        {
            try
            {
                _logger.LogInformation("Iniciando busca de subcategorias. ID: {ID}",
                    id);

                var subcategorias = await _subCategoriaService.BuscarSubCategorias(id);

                if (subcategorias == null || !subcategorias.Any())
                {
                    _logger.LogInformation("Nenhuma subcategoria encontrada com os filtros informados.");
                    return NotFound("Nenhuma subcategoria encontrada.");
                }

                _logger.LogInformation("Busca concluída. Total encontrado: {Quantidade}", subcategorias.ToList().Count);
                return Ok(subcategorias);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Parâmetro inválido na busca de subcategorias.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar subcategorias.");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }


        [HttpPut("EditarSubCategoria/{ID}")]
        [SwaggerOperation(Summary = "Editar subcategorias", OperationId = "Put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarSubCategoria(int ID, [FromBody] SubCategoriaDto subcategoria)
        {
            try
            {
                SubCategoria categoriaEditada = await _subCategoriaService.EditarSubCategoria(ID, subcategoria);
                if (categoriaEditada != null)
                {
                    return Ok(categoriaEditada);
                }
                return NotFound($"Nenhuma subcategoria encontrada com os parametros {subcategoria}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("ExcluirSubCategoria/{ID}")]
        [SwaggerOperation(Summary = "Excluir subcategorias", OperationId = "Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExcluirSubCategoria(int ID)
        {
            try
            {
                SubCategoria categoriaEditada = await _subCategoriaService.ExcluirSubCategoria(ID);
                if (categoriaEditada == null)
                {
                    return NotFound($"Nenhuma subcategoria foi encontrada para o ID:{ID}");
                }

                return Ok($"A categoria com o ID:{ID} foi excluída.");

            }
            catch (ArgumentOutOfRangeException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
