using ApplicationServices.Services;
using Domain.Dtos.CategoriaDtos;
using Domain.Dtos.SubCategoriaDtos;
using Domain.Exceptions;
using Domain.Exceptions.SubCategoriaException;
using Domain.Models;
using Domain.Repositorys;
using Domain.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class SubCategoriaServiceTest
    {
        private readonly ISubCategoriaRepository _subCategoriaRepositoryMock = Substitute.For<ISubCategoriaRepository>();
        private readonly ICategoriaRepository _categoriaRepository = Substitute.For<ICategoriaRepository>();
        private readonly ILogger<SubCategoriaService> _logger = Substitute.For<ILogger<SubCategoriaService>>();
        private readonly IValidator<string> _validator = Substitute.For<IValidator<string>>();
        private readonly SubCategoriaService _subCategoriaService;

        public SubCategoriaServiceTest()
        {

            _subCategoriaService = new SubCategoriaService(_subCategoriaRepositoryMock, _logger,_categoriaRepository, _validator);
            
        }

        [Fact]
        public async Task SalvarSubCategoria_Valido_DeveSalvarERetornarCategoria()
        {
            // Arrange

            var subCategoriaEsperada = new SubCategoriaDto {  Nome = "Bebidas", CategoriaId = 1 };

            _subCategoriaRepositoryMock.CriarSubCategoriaAsync(Arg.Any<SubCategoria>()).Returns(1);
            var validationResult = new ValidationResult();
            _validator.Validate(subCategoriaEsperada.Nome).Returns(validationResult);
            _categoriaRepository.BuscarCategoriaPorIdAsync(Arg.Any<int>()).Returns(new Categoria());

            // Act
            var resultado = await _subCategoriaService.CriarSubCategoria(subCategoriaEsperada);

            // Assert
            Assert.Equal(subCategoriaEsperada.Nome, resultado.Nome);
            Assert.True(resultado.Status);

        }

        [Fact]
        public async Task SalvarSubCategoria_NomeVazio_DeveLancarArgumentException()
        {
            // Arrange
            var subCategoriaEsperada = new SubCategoriaDto { Nome = "  " }; ;
            // Act

            _categoriaRepository.BuscarCategoriaPorIdAsync(Arg.Any<int>()).Returns(new Categoria());
            _validator.Validate(subCategoriaEsperada.Nome).Returns(new ValidationResult(
       
            new List<ValidationFailure>
            {
                new ValidationFailure("Nome", "O nome é obrigatório")
            }));

            // Act + Assert
            var ex = await Assert.ThrowsAsync<ObjectNotFilledException>(async () =>
            {
                await _subCategoriaService.CriarSubCategoria(subCategoriaEsperada);
            });
        }

        [Fact]
        public async Task BuscarSubCategorias_ListaVazia_DeveLancarSubCategoriaNotFoundException()
        {
            // Arrange
           _subCategoriaRepositoryMock.BuscarSubCategoriasAsync(Arg.Any<int>()).ReturnsNull();

            // Act
            var result = await Assert.ThrowsAsync<SubCategoriaNotFoundException>(async () =>
                  await _subCategoriaService.BuscarSubCategorias(1));

            Assert.Equal("Ops a subcategoria não foi encontrado", result.Message);
        }

        [Fact]
        public async Task BuscarSubCategorias_ParametrosValidos_DeveChamarRepositorio()
        {
            // Arrange
            var categorias = new List<SubCategoria>
            {
                new SubCategoria {  Nome = "Bebidas", Status = true },
                new SubCategoria {  Nome = "Comidas", Status = true }
            };
            _subCategoriaRepositoryMock.BuscarSubCategoriasAsync(Arg.Any<int?>()).Returns(categorias);
            // Act
            var resultado = await _subCategoriaService.BuscarSubCategorias(null);
            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
        }

        [Fact]
        public async Task EditarCategoria_Valido_DeveSalvarERetornarCategoria()
        {
            // Arrange

            var subcategoriaEsperada = new SubCategoriaDto { Nome = "Bebidas" };


            _subCategoriaRepositoryMock.CriarSubCategoriaAsync(Arg.Any<SubCategoria>()).Returns(1);
            var categoria = new SubCategoria() { ID = 1, Nome = "Novo", Status = true };
            var validationResult = new ValidationResult();
            _validator.Validate(subcategoriaEsperada.Nome).Returns(validationResult);
            _categoriaRepository.BuscarCategoriaPorIdAsync(Arg.Any<int>()).Returns(new Categoria());
            _subCategoriaRepositoryMock.BuscarSubCategoriaPorIdAsync(Arg.Any<int>()).Returns(categoria);
            _subCategoriaRepositoryMock.AtualizarSubCategoriaAsync(categoria).Returns(categoria);
            // Act
            var resultado = await _subCategoriaService.EditarSubCategoria(1, subcategoriaEsperada);

            // Assert
            Assert.Equal(subcategoriaEsperada.Nome, resultado.Nome);
            Assert.True(resultado.Status);

        }

        [Fact]
        public async Task ExcluiSubCategoria_DeveSalvarERetornarSubCategoria()
        {
            // Arrange

            var categoria = new SubCategoria() { ID = 1, Nome = "Novo", Status = true };

            _subCategoriaRepositoryMock.BuscarSubCategoriaPorIdAsync(Arg.Any<int>()).Returns(categoria);
            _subCategoriaRepositoryMock.ExcluirSubCategoriaAsync(categoria).Returns(categoria);
            // Act
            var resultado = await _subCategoriaService.ExcluirSubCategoria(categoria.ID);

            // Assert
            Assert.NotNull(resultado);
            await _subCategoriaRepositoryMock.Received(1).BuscarSubCategoriaPorIdAsync(categoria.ID);
            await _subCategoriaRepositoryMock.Received(1).ExcluirSubCategoriaAsync(categoria);

        }
    }
}
