using Moq;
using Microsoft.AspNetCore.Mvc;
using Domain.Services;
using AplicacaoProjeto.Controllers;
using Domain.Models;
using Microsoft.Extensions.Logging;

public class CategoriaControllerTests
{
    private readonly Mock<ICategoriaService> _categoriaServiceMock;
    private readonly Mock<ILogger<CategoriaController>> _logger;
    private readonly CategoriaController _controller;

    public CategoriaControllerTests()
    {
        _categoriaServiceMock = new Mock<ICategoriaService>();
        _logger = new Mock<ILogger<CategoriaController>>();
        _controller = new CategoriaController(_categoriaServiceMock.Object, _logger.Object);
    }

    //[Fact]
    //public async Task SalvarCategoria_RetornaOkComCategoria()
    //{
    //    // Arrange
    //    var categoria = new Categoria { ID = 1, Nome = "Bebidas" };
    //    _categoriaServiceMock
    //        .Setup(s => s.CriarCategoria(It.IsAny<string>()))
    //        .ReturnsAsync(categoria);

    //    // Act
    //    var result = await _controller.CriarCategoria("Bebidas");

    //    // Assert
    //    var okResult = Assert.IsType<CreatedResult>(result);
    //    var retorno = Assert.IsType<Categoria>(okResult.Value);
    //    Assert.Equal("Bebidas", retorno.Nome);
    //}

    //[Fact]
    //public async Task BuscarCategorias_RetornaOkComLista()
    //{
    //    // Arrange
    //    var lista = new List<Categoria>
    //    {
    //        new Categoria { ID = 1, Nome = "Bebidas" },
    //        new Categoria { ID = 2, Nome = "Comidas" }
    //    };
    //    _categoriaServiceMock
    //        .Setup(s => s.BuscarCategorias(null, null, null, null, "ASC"))
    //        .ReturnsAsync(lista);

    //    // Act
    //    var result = await _controller.BuscarCategorias(null, null, null, null, "ASC");

    //    // Assert
    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    var retorno = Assert.IsAssignableFrom<IEnumerable<Categoria>>(okResult.Value);
    //    Assert.Equal(2, ((List<Categoria>)retorno).Count);
    //}
}
