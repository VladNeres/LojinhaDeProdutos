using ApplicationServices.Services;
using Castle.Core.Logging;
using Domain.Models;
using Domain.Repositorys;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;

namespace Tests.TesteServices
{
    [TestClass]
    public class CategoriaServiceUnitTest
    {
        private readonly ICategoriaRepository _categoriaRepositoryMock = Substitute.For<ICategoriaRepository>();
        private readonly ILogger<CategoriaService> _logger = Substitute.For<ILogger<CategoriaService>>();
        private readonly IValidator<string> _validator = Substitute.For<IValidator<string>>();
        private readonly CategoriaService _categoriaService;

        public CategoriaServiceUnitTest()
        {

            _categoriaService = new CategoriaService(_categoriaRepositoryMock, _logger, _validator);
        }

        [TestMethod]
        public async Task SalvarCategoria_Valido_DeveSalvarERetornarCategoria()
        {
            // Arrange
           
            var categoriaEsperada = new Categoria { ID = 1, Nome = "Bebidas", Status = true };

            _categoriaRepositoryMock.CriarCategoriaAsync(Arg.Any<Categoria>()).Returns(categoriaEsperada.ID);
                

            // Act
            var resultado = await _categoriaService.CriarCategoria(categoriaEsperada.Nome);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(categoriaEsperada.Nome, resultado.Nome);
            Assert.IsTrue(resultado.Status);
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task SalvarCategoria_NomeVazio_DeveLancarArgumentException()
        {
            // Arrange
            string nomeInvalido = "   ";

            // Act
            await _categoriaService.CriarCategoria(nomeInvalido);

            // Assert é tratado pelo ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task BuscarCategorias_OrdenacaoInvalida_DeveLancarArgumentException()
        {
            // Arrange
            string ordenacaoInvalida = "INVALID";

            // Act
            await _categoriaService.BuscarCategorias(null, null, null, null, ordenacaoInvalida);

            // Assert é tratado pelo ExpectedException
        }

        [TestMethod]
        public async Task BuscarCategorias_ParametrosValidos_DeveChamarRepositorio()
        {
            // Arrange
            var categorias = new List<Categoria>
            {
                new Categoria { ID = 1, Nome = "Bebidas", Status = true },
                new Categoria { ID = 2, Nome = "Comidas", Status = true }
            };

            //_categoriaRepositoryMock
            //    .Setup(r => r.BuscarCategoriasAsync(null, null, null, "ID", "ASC"))
            //    .ReturnsAsync(categorias);

            // Act
            var resultado = await _categoriaService.BuscarCategorias(null, null, null, null, "ASC");

            // Assert
            //Assert.IsNotNull(resultado);
            //Assert.AreEqual(2, resultado.Count());
            //_categoriaRepositoryMock.Verify(r => r.BuscarCategoriasAsync(null, null, null, "ID", "ASC"), Times.Once);
        }
    }

}
