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
        public SubCategoriaController(ISubCategoriaService categoriaService, ILogger<SubCategoriaController> logger)
        {
            _subCategoriaService = categoriaService;
            _logger = logger;
        }

        [HttpPost("CriarSubCategoria")]
        [SwaggerOperation(Summary = "Criar Subcategorias", OperationId = "Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarSubCategoria([FromBody] SubCategoria subCategoria)
        {
            try
            {
                _logger.LogInformation("Iniciando o cadastro da subcategoria: {NomeCategoria}", subCategoria);

                SubCategoria categoria = await _subCategoriaService.CriarSubCategoria(subCategoria);

                if (categoria != null)
                {
                    _logger.LogInformation("Categoria cadastrada com sucesso. ID: {ID}", categoria.Id);
                    return Created();
                }

                _logger.LogWarning("Falha ao cadastrar categoria. Nome: {NomeCategoria}", subCategoria);
                return BadRequest($"Não foi possível cadastrar a categoria. CategoriaNome: {subCategoria}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar categoria: {NomeCategoria}", subCategoria);
                return StatusCode(500, new { erro = "Ocorreu um erro ao processar a requisição." });
            }
        }

        [HttpGet("BuscarSubCategoria")]
        [SwaggerOperation(Summary = "Buscar subcategorias", OperationId = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarCategorias(
            [FromQuery] int? id,
            [FromQuery] string? nome,
            [FromQuery] bool? status,
            [FromQuery] string? ordenarPor,
            [FromQuery] string? tipoOrdenacao)
        {
            try
            {
                _logger.LogInformation("Iniciando busca de subcategorias. ID: {ID}, Nome: {Nome}, Status: {Status}, OrdenarPor: {OrdenarPor}, Ordenacao: {Ordenacao}",
                    id, nome, status, ordenarPor, tipoOrdenacao);

                var categorias = await _subCategoriaService.BuscarSubCategorias(id, nome, status, ordenarPor, tipoOrdenacao);

                if (categorias == null || !categorias.Any())
                {
                    _logger.LogInformation("Nenhuma subcategoria encontrada com os filtros informados.");
                    return NotFound("Nenhuma categoria encontrada.");
                }

                _logger.LogInformation("Busca concluída. Total encontrado: {Quantidade}", categorias.ToList().Count);
                return Ok(categorias);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Parâmetro inválido na busca de categorias.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar categorias.");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }


        [HttpPut("EditarSubCategoria/{ID}")]
        [SwaggerOperation(Summary = "Editar subcategorias", OperationId = "Put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarCategoria(int ID, [FromBody] SubCategoriaDto subcategoria)
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


        [HttpDelete("ExcluirCategoria/{ID}")]
        [SwaggerOperation(Summary = "Excluir subcategorias", OperationId = "Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExcluirCategoria(int ID)
        {
            try
            {
                SubCategoria categoriaEditada = await _subCategoriaService.ExcluirSubCategoria(ID);
                if (categoriaEditada == null)
                {
                    return NotFound($"Nenhuma categoria foi encontrada para o ID:{ID}");
                }

                return Ok($"A categoria com o ID:{ID} foi excluída.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
