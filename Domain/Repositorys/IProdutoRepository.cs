using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositorys
{
    public interface IProdutoRepository
    {
        Task<int> CriarProdutoAsync(Produto produto);
        Task<Produto> AtualizarProdutoAsync(Produto produto);
        Task<Produto> ExcluirProdutoAsync(Produto? produtoID);
        Task<IEnumerable<Produto>> BuscarProdutosAsync(int? ID);
        Task<Produto> BuscarProdutoPorIdAsync(int id);
    }
}
