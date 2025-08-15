using Domain.Dtos.CategoriaDtos;
using Domain.Dtos.SubCategoriaDtos;
using Domain.Models;

namespace Domain.Services
{
    public interface ISubCategoriaService
    {
        Task<SubCategoria> CriarSubCategoria(SubCategoriaDto SubCategoria);
        Task<IEnumerable<SubCategoria>> BuscarSubCategorias(int? ID);
        Task<SubCategoria> EditarSubCategoria(int ID, SubCategoriaDto categoria);
        Task<SubCategoria> ExcluirSubCategoria(int ID);

    }
}
