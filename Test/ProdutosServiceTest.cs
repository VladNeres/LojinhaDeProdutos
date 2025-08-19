using ApplicationServices.Services;
using DataAccess.Repositorys;
using Domain.Dtos.ProdutoDtos;
using Domain.Exceptions;
using Domain.Exceptions.ProdutoException;
using Domain.Models;
using Domain.Repositorys;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Test
{
    public class ProdutosServiceTest
    {
        private readonly IProdutoRepository _produtoRepositoryMock = Substitute.For<IProdutoRepository>();
        private readonly ISubCategoriaRepository _subCategoriaRepository = Substitute.For<ISubCategoriaRepository>();
        private readonly ILogger<ProdutoService> _logger = Substitute.For<ILogger<ProdutoService>>();
        private readonly IValidator<string> _validator = Substitute.For<IValidator<string>>();
        private readonly ProdutoService _produtoService;

        public ProdutosServiceTest()
        {

            _produtoService = new ProdutoService(_produtoRepositoryMock, _logger, _validator, _subCategoriaRepository);
        }

        [Fact]
        public async Task SalvarProduto_Valido_DeveSalvarERetornarProduto()
        {
            // Arrange

            var produtoEsperado = new ProdutoDto { Nome = "Bola", Status = true, DataCriacao = DateTime.Now, Descricao = "para brincar" };

            _produtoRepositoryMock.CriarProdutoAsync(Arg.Any<Produto>()).Returns(1);
            var validationResult = new ValidationResult();
            _validator.Validate(produtoEsperado.Nome).Returns(validationResult);
            _subCategoriaRepository.BuscarSubCategoriaPorIdAsync(Arg.Any<int>()).Returns(new SubCategoria());

            // Act
            var resultado = await _produtoService.CriarProduto(produtoEsperado);

            // Assert
            Assert.Equal(produtoEsperado.Nome, resultado.Nome);
            Assert.True(resultado.Status);

        }

        [Fact]
        public async Task SalvarProduto_NomeVazio_DeveLancarArgumentException()
        {
            // Arrange
            var produtoEsperado = new ProdutoDto { Nome = "Bola", Status = true, DataCriacao = DateTime.Now, Descricao = "para brincar" };
            // Act


            _validator.Validate(produtoEsperado.Nome).Returns(new ValidationResult(
            new List<ValidationFailure>
            {
                new ValidationFailure("Nome", "O nome é obrigatório")
            }));

            // Act + Assert
            var ex = await Assert.ThrowsAsync<ObjectNotFilledException>(async () =>
            {
                await _produtoService.CriarProduto(produtoEsperado);
            });
        }

        [Fact]
        public async Task BuscarProdutos_OrdenacaoInvalida_DeveLancarArgumentException()
        {
            // Arrange
           _produtoRepositoryMock.BuscarProdutosAsync(Arg.Any<int>()).ReturnsNull();

            // Act
            var result = await Assert.ThrowsAsync<ProdutoNotFoundException>(async () =>
                  await _produtoService.BuscarProdutos(1));

            Assert.Equal("Ops o produto não foi encontrado", result.Message);
        }

        [Fact]
        public async Task BuscarProdutos_ParametrosValidos_DeveChamarRepositorio()
        {
            // Arrange
            var produtos = new List<Produto>
            {
                new Produto {  Nome = "Bebidas", Status = true },
                new Produto {  Nome = "Comidas", Status = true }
            };
            _produtoRepositoryMock.BuscarProdutosAsync(Arg.Any<int?>()).Returns(produtos);
            // Act
            var resultado = await _produtoService.BuscarProdutos(1);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }
    }
}

