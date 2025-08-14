using Domain.Dtos.CategoriaDtos;
using Domain.Models;

namespace Domain.Services
{
    public interface ICategoriaService
    {
        Task<Categoria> CriarCategoria(string nomeCategoria);
        Task<IEnumerable<Categoria>> BuscarCategorias(int? ID, string? nome, bool? status, string? ordenarPor, string tipoOrdenacao);
        Task<Categoria> EditarCategoria(int ID, CategoriaDto categoria);
        Task<Categoria> ExcluirCategoria(int iD);
    }
}
