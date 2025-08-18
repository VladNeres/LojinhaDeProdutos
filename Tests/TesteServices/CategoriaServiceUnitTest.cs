using ApplicationServices.Services;
using Castle.Core.Logging;
using Domain.Models;
using Domain.Repositorys;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.TesteServices
{
    [TestClass]
    public class CategoriaServiceUnitTest
    {
        private readonly Mock<ICategoriaRepository> _categoriaRepositoryMock;
        private readonly Mock<ILogger<CategoriaService>> _logger;
        private readonly CategoriaService _categoriaService;

        public CategoriaServiceUnitTest()
        {
            _categoriaRepositoryMock = new Mock<ICategoriaRepository>();
            _logger = new Mock<ILogger<CategoriaService>>();
            _categoriaService = new CategoriaService(_categoriaRepositoryMock.Object, _logger.Object);
        }

        [TestMethod]
        public async Task SalvarCategoria_Valido_DeveSalvarERetornarCategoria()
        {
            // Arrange
            string nome = "Bebidas";
            var categoriaEsperada = new Categoria { ID = 1, Nome = nome, Status = true };

               _categoriaRepositoryMock
                .Setup(r =>  r.CriarCategoriaAsync(It.IsAny<Categoria>()))
                .ReturnsAsync(categoriaEsperada.ID);

            // Act
            var resultado = await _categoriaService.CriarCategoria(nome);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(nome, resultado.Nome);
            Assert.IsTrue(resultado.Status);
            _categoriaRepositoryMock.Verify(r => r.CriarCategoriaAsync(It.IsAny<Categoria>()), Times.Once);
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

            _categoriaRepositoryMock
                .Setup(r => r.BuscarCategoriasAsync(null, null, null, "ID", "ASC"))
                .ReturnsAsync(categorias);

            // Act
            var resultado = await _categoriaService.BuscarCategorias(null, null, null, null, "ASC");

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count());
            _categoriaRepositoryMock.Verify(r => r.BuscarCategoriasAsync(null, null, null, "ID", "ASC"), Times.Once);
        }
    }

}
