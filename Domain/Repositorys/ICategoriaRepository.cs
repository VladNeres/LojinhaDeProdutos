using ApplicationServices.Dtos;
using Domain.Models;

namespace Domain.Repositorys
{
    public interface ICategoriaRepository
    {
        Task<Categoria> CriarCategoriaAsync(Categoria categoria);
        Task<Categoria> AtualizarCategoriaAsync(Categoria categoria);
        Task<Categoria> ExcluirCategoriaAsync(Categoria categoria);
        Task<Categoria> BuscarNomeCategoriaAsync(string nome);
        Task<Categoria> BuscarCategoriaPorIdAsync(int id);
        Task<IEnumerable<Categoria>> BuscarCategoriasAsync(int? ID, string? nome, bool? status, string? ordenarPor, string tipoOrdenacao);
        Task<IEnumerable<SubCategoria>> BuscarSubCategoriasPorIdAsync(int categoriaId);
        
    }
}
