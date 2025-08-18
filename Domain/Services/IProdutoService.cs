using Domain.Dtos.ProdutoDtos;
using Domain.Models;
namespace Domain.Services
{
    public interface IProdutoService
    {
        Task<Produto> CriarProduto(ProdutoDto produto);
        Task<IEnumerable<Produto>> BuscarProdutos(int? ID);
        Task<Produto> EditarProduto(int ID, ProdutoDto categoria);
        Task<Produto> ExcluirProduto(int ID);
    }
}
