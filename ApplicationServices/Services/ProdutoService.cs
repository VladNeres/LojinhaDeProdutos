using Castle.Core.Logging;
using DataAccess.Repositorys;
using Domain.Dtos.ProdutoDtos;
using Domain.Exceptions;
using Domain.Exceptions.ProdutoException;
using Domain.Mapper;
using Domain.Models;
using Domain.Repositorys;
using Domain.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;


namespace ApplicationServices.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ILogger<ProdutoService> _logger;
        private readonly IValidator<string> _validator;
        public ProdutoService(IProdutoRepository produtoRepository, ILogger<ProdutoService> logger)
        {
            _produtoRepository = produtoRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Produto>> BuscarProdutos(int? ID)
        {
            _logger.LogInformation("Iniciando busca de produtos. ID: {ID}",
            ID);

            var resultado = await _produtoRepository.BuscarProdutosAsync(ID);

            _logger.LogInformation("Busca concluída. Total encontrado: {Quantidade}", resultado.ToList().Count);

            return resultado;
        }

        public async Task<Produto> CriarProduto(ProdutoDto produtoDto)
        {
            if (produtoDto is null)
                throw new ObjectNotFilledException();

            var result = _validator.Validate(produtoDto.Nome);

            if (!result.IsValid)
                throw new ObjectNotFilledException(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));


            Produto produto = produtoDto.ParaProduto();
            
            await _produtoRepository.CriarProdutoAsync(produto);
            return produto;

        }

        public async Task<Produto> EditarProduto(int ID, ProdutoDto produtoDto)
        {
            if(produtoDto is null)
                throw new ObjectNotFilledException();

          var produto =  await _produtoRepository.BuscarProdutoPorIdAsync(ID);
            var result = _validator.Validate(produtoDto.Nome);

            if (!result.IsValid)
                throw new ObjectNotFilledException(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));

            produto.AtualizarComProdutoDto(produtoDto);
           return await  _produtoRepository.AtualizarProdutoAsync(produto);
        }

        public async Task<Produto> ExcluirProduto(int ID)
        {
           var produto =  await _produtoRepository.BuscarProdutoPorIdAsync(ID);

            if (produto is null)
                throw new ProdutoNotFoundException();

         return   await  _produtoRepository.ExcluirProdutoAsync(produto);
        }
    }
}
