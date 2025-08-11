using Domain.Models;

namespace Domain.Repositorys
{
    public interface ICategoriaRepository
    {
        Task<int> CadastrarCategoria(Categoria categoria);
        Task<IEnumerable<Categoria>> BuscarCategorias(int? ID, string? nome, bool? status, string? ordenarPor, string ordenacao);
    }
}
