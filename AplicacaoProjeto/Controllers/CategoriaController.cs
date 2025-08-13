using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AplicacaoProjeto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly ILogger<CategoriaController> _logger;
        public CategoriaController(ICategoriaService categoriaService, ILogger<CategoriaController> logger)
        {
            _categoriaService = categoriaService;
            _logger = logger;
        }

        [HttpPost("CriarCategoria")]
        [SwaggerOperation(Summary = "Criar categorias", OperationId = "Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarCategoria([FromBody] string nomeCategoria)
        {
            try
            {
                _logger.LogInformation("Iniciando o cadastro da categoria: {NomeCategoria}", nomeCategoria);

                Categoria categoria = await _categoriaService.CriarCategoria(nomeCategoria);

                if (categoria != null)
                {
                    _logger.LogInformation("Categoria cadastrada com sucesso. ID: {ID}", categoria.ID);
                    return Created();
                }

                _logger.LogWarning("Falha ao cadastrar categoria. Nome: {NomeCategoria}", nomeCategoria);
                return BadRequest($"Não foi possível cadastrar a categoria. CategoriaNome: {nomeCategoria}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar categoria: {NomeCategoria}", nomeCategoria);
                return StatusCode(500, new { erro = "Ocorreu um erro ao processar a requisição." });
            }
        }

        [HttpGet("BuscarCategoria")]
        [SwaggerOperation(Summary = "Buscar categorias", OperationId = "Get")]
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
                _logger.LogInformation("Iniciando busca de categorias. ID: {ID}, Nome: {Nome}, Status: {Status}, OrdenarPor: {OrdenarPor}, Ordenacao: {Ordenacao}",
                    id, nome, status, ordenarPor, tipoOrdenacao);

                var categorias = await _categoriaService.BuscarCategorias(id, nome, status, ordenarPor, tipoOrdenacao);

                if (categorias == null || !categorias.Any())
                {
                    _logger.LogInformation("Nenhuma categoria encontrada com os filtros informados.");
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


        [HttpPut("EditarCategoria/{ID}")]
        [SwaggerOperation(Summary = "Editar categorias", OperationId = "Put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarCategoria(int ID, [FromBody] Categoria categoria)
        {
            try
            {
                Categoria categoriaEditada = await _categoriaService.EditarCategoria(ID,categoria);
                if (categoriaEditada != null)
                {
                    return Ok(categoriaEditada);
                }
                return NotFound($"Nenhuma categoria encontrada com os parametros {categoria}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPatch("EditarStatusCategoria/{ID}")]
        //[SwaggerOperation(Summary = "Editar status das categorias", OperationId = "Patch")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> EditarStatusCategoria(int ID)
        //{
        //    try
        //    {
        //        Categoria categoriaEditada = await _categoriaService.EditarStatusCategoria(ID);
        //        if (categoriaEditada != null)
        //        {
        //            return Ok(categoriaEditada);
        //        }
        //        if(categoriaEditada == null)
        //        {
        //            return NotFound($"Nenhuma categoria foi encontrada para o ID:{ID}");
        //        }
        //        return BadRequest($"Não foi possível editar o status da categoria {ID}.");

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpDelete("ExcluirCategoria/{ID}")]
        //[SwaggerOperation(Summary = "Excluir categorias", OperationId = "Delete")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> ExcluirCategoria(int ID)
        //{
        //    try
        //    {
        //        Categoria categoriaEditada = await _categoriaService.ExcluirCategoria(ID);
        //        if (categoriaEditada == null)
        //        {
        //            return NotFound($"Nenhuma categoria foi encontrada para o ID:{ID}");
        //        }

        //        return Ok($"A categoria com o ID:{ID} foi excluída.");

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
