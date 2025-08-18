using Domain.Dtos.ProdutoDtos;
using Domain.Exceptions;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AplicacaoProjeto.Controllers
{
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly ILogger<ProdutoController> _logger;
        public ProdutoController(IProdutoService produtoService, ILogger<ProdutoController> logger)
        {
            _produtoService = produtoService;
            _logger = logger;
        }

        [HttpPost("CriarProduto")]
        [SwaggerOperation(Summary = "Criar Produtos", OperationId = "Post")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CriarProduto([FromBody] ProdutoDto produto)
        {
            try
            {
                _logger.LogInformation($"Iniciando o cadastro da produto: {produto.Nome}", produto);

                Produto prod = await _produtoService.CriarProduto(produto);

                if (produto != null)
                {
                    _logger.LogInformation("Produto cadastrada com sucesso. ID: {ID}", prod.Id);
                    return Created();
                }

                _logger.LogWarning($"Falha ao cadastrar produto. Nome: {prod.Nome}", produto);
                return BadRequest($"Não foi possível cadastrar a produto. {prod.Nome}.");
            }
            catch (ObjectNotFilledException ex)
            {
                _logger.LogError(ex, "Erro ao salvar produto: {produto.Nome}");
                return StatusCode(500, new { erro = "Ocorreu um erro ao processar a requisição." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao salvar produto: {produto.Nome}");
                return StatusCode(500, new { erro = "Ocorreu um erro ao processar a requisição." });
            }
        }

        [HttpGet("BuscarProduto")]
        [SwaggerOperation(Summary = "Buscar produtos", OperationId = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BuscarProdutos(
            [FromQuery] int? id)
        {
            try
            {
                _logger.LogInformation("Iniciando busca de produtos. ID: {ID}",
                    id);

                var produtos = await _produtoService.BuscarProdutos(id);

                if (produtos == null || !produtos.Any())
                {
                    _logger.LogInformation("Nenhuma produto encontrado com os filtros informados.");
                    return NotFound("Nenhuma produto encontrado.");
                }

                _logger.LogInformation("Busca concluída. Total encontrado: {Quantidade}", produtos.ToList().Count);
                return Ok(produtos);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Parâmetro inválido na busca de produtos.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar produtos.");
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }


        [HttpPut("EditarProduto/{ID}")]
        [SwaggerOperation(Summary = "Editar produtos", OperationId = "Put")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarProduto(int ID, [FromBody] ProdutoDto produto)
        {
            try
            {
                Produto produtoEditada = await _produtoService.EditarProduto(ID, produto);
                if (produtoEditada != null)
                {
                    return Ok(produtoEditada);
                }
                return NotFound($"Nenhuma produto encontrado com os parametros {produto}.");
            }
            catch (ObjectNotFilledException ex)
            {
                _logger.LogError("Ocorreu um erro ao atualizar produto");
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("ExcluirProduto/{ID}")]
        [SwaggerOperation(Summary = "Excluir produtos", OperationId = "Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExcluirProduto(int ID)
        {
            try
            {
                Produto produtoEditada = await _produtoService.ExcluirProduto(ID);
                if (produtoEditada == null)
                {
                    return NotFound($"Nenhum produto foi encontrado para o ID:{ID}");
                }

                return Ok($"O produto com o ID:{ID} foi excluído.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
