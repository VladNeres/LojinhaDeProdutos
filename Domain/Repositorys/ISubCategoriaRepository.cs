using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositorys
{
    public interface ISubCategoriaRepository
    {
        Task<int> CriarSubCategoriaAsync(SubCategoria categoria);
        Task<SubCategoria> AtualizarSubCategoriaAsync(SubCategoria categoria);
        Task<SubCategoria> ExcluirSubCategoriaAsync(SubCategoria categoria);
        Task<SubCategoria> BuscarNomeSubCategoriaAsync(string nome);
        Task<SubCategoria> BuscarSubCategoriaPorIdAsync(int id);
        Task<IEnumerable<SubCategoria>> BuscarSubCategoriasAsync(int? ID, string? nome, bool? status, string? ordenarPor, string tipoOrdenacao);
        Task<IEnumerable<SubCategoria>> BuscarSubCategoriasPorIdAsync(int categoriaId);
    }
}
