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
        Task<SubCategoria> ExcluirSubCategoriaAsync(SubCategoria? categoria);
        Task<IEnumerable<SubCategoria>> BuscarSubCategoriasAsync(int? ID);
        Task<SubCategoria> BuscarSubCategoriaPorIdAsync(int id);
    }
}
