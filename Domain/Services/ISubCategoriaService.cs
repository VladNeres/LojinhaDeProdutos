using Domain.Dtos.CategoriaDtos;
using Domain.Dtos.SubCategoriaDtos;
using Domain.Models;

namespace Domain.Services
{
    public interface ISubCategoriaService
    {
        Task<SubCategoria> CriarSubCategoria(SubCategoria SubCategoria);
        Task<IEnumerable<SubCategoria>> BuscarSubCategorias(int? ID, string? nome, bool? status, string? ordenarPor, string tipoOrdenacao);
        Task<SubCategoria> EditarSubCategoria(int ID, SubCategoriaDto categoria);
        Task<SubCategoria> ExcluirSubCategoria(int ID);
    }
}
