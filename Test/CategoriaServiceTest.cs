using ApplicationServices.Services;
using DataAccess.Repositorys;
using Domain.Dtos.CategoriaDtos;
using Domain.Exceptions;
using Domain.Models;
using Domain.Repositorys;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Runtime.ConstrainedExecution;

namespace Test
{
    public class CategoriaServiceTest
    {

        private readonly ICategoriaRepository _categoriaRepositoryMock = Substitute.For<ICategoriaRepository>();
        private readonly ILogger<CategoriaService> _logger = Substitute.For<ILogger<CategoriaService>>();
        private readonly IValidator<string> _validator = Substitute.For<IValidator<string>>();
        private readonly CategoriaService _categoriaService;

        public CategoriaServiceTest()
        {

            _categoriaService = new CategoriaService(_categoriaRepositoryMock, _logger, _validator);
        }

        [Fact]
        public async Task SalvarCategoria_Valido_DeveSalvarERetornarCategoria()
        {
            // Arrange

            var categoriaEsperada = new Categoria { ID = 1, Nome = "Bebidas", Status = true };

            _categoriaRepositoryMock.CriarCategoriaAsync(Arg.Any<Categoria>()).Returns(categoriaEsperada.ID);
            var validationResult = new ValidationResult();
            _validator.Validate(categoriaEsperada.Nome).Returns(validationResult);

            // Act
            var resultado = await _categoriaService.CriarCategoria(categoriaEsperada.Nome);

            // Assert
            Assert.Equal(categoriaEsperada.Nome, resultado.Nome);
            Assert.True(resultado.Status);

        }

        [Fact]
        public async Task SalvarCategoria_NomeVazio_DeveLancarArgumentException()
        {
            // Arrange
            string nomeInvalido = "   ";
            // Act


            _validator.Validate(nomeInvalido).Returns(new ValidationResult(
       new List<ValidationFailure>
       {
            new ValidationFailure("Nome", "O nome é obrigatório")
       }));

            // Act + Assert
            var ex = await Assert.ThrowsAsync<ObjectNotFilledException>(async () =>
            {
                await _categoriaService.CriarCategoria(nomeInvalido);
            });
        }

        [Fact]
        public async Task BuscarCategorias_OrdenacaoInvalida_DeveLancarArgumentException()
        {
            // Arrange
            string ordenacaoInvalida = "INVALID";

            // Act
            var result = await Assert.ThrowsAsync<ArgumentException>(async () =>
                  await _categoriaService.BuscarCategorias(null, null, null, null, ordenacaoInvalida));

            Assert.Equal("O parâmetro 'ordenacao' deve ser 'ASC' ou 'DESC'.", result.Message);
        }

        [Fact]
        public async Task BuscarCategorias_ParametrosValidos_DeveChamarRepositorio()
        {
            // Arrange
            var categorias = new List<Categoria>
            {
                new Categoria { ID = 1, Nome = "Bebidas", Status = true },
                new Categoria { ID = 2, Nome = "Comidas", Status = true }
            };
            _categoriaRepositoryMock.BuscarCategoriasAsync(Arg.Any<int?>(),Arg.Any<string>(),Arg.Any<bool?>(),Arg.Any<string>(),Arg.Any<string>()).Returns(categorias);
            // Act
            var resultado = await _categoriaService.BuscarCategorias(null, null, null, null, "ASC");
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }


        [Fact]
        public async Task EditarCategoria_Valido_DeveSalvarERetornarCategoria()
        {
            // Arrange

            var categoriaEsperada = new CategoriaDto { Nome = "Bebidas" };


            _categoriaRepositoryMock.CriarCategoriaAsync(Arg.Any<Categoria>()).Returns(1);
            var categoria = new Categoria() { ID = 1, Nome = "Novo", Status = true };
            var validationResult = new ValidationResult();
            _validator.Validate(categoriaEsperada.Nome).Returns(validationResult);
            _categoriaRepositoryMock.BuscarCategoriaPorIdAsync(Arg.Any<int>()).Returns(categoria);
            _categoriaRepositoryMock.AtualizarCategoriaAsync(categoria).Returns(categoria);
            // Act
            var resultado = await _categoriaService.EditarCategoria(1, categoriaEsperada);

            // Assert
            Assert.Equal(categoriaEsperada.Nome, resultado.Nome);
            Assert.True(resultado.Status);

        }
    }
}
