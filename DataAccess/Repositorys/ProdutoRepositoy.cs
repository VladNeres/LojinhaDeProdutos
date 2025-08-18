using Domain.Models;
using Domain.Repositorys;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.Repositorys
{
    public class ProdutoRepositoy : IProdutoRepository
    {
        private readonly DatabaseContext _context;

        public ProdutoRepositoy(DatabaseContext context)
        {
            _context = context;
        }

        public async  Task<Produto> BuscarProdutoPorIdAsync(int id)
        {
            return await _context.Produtos
               .Where(s => s.Id == id)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Produto>> BuscarProdutosAsync(int? ID)
        {
            var idParam = new SqlParameter("@ID", ID ?? (object)DBNull.Value);


            var resultado = await _context.Produtos
                .FromSqlRaw(
                    "EXEC SubCategoria_BuscarSubCategoria @ID",
                    idParam)
                .ToListAsync();

            return resultado;
        }
        public async Task<int> CriarProdutoAsync(Produto produto)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "dbo.SubCategoria_CadastrarSubCategoria";
            command.CommandType = CommandType.StoredProcedure;
            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteScalarAsync(); // retorna apenas o valor da primeira coluna
            return Convert.ToInt32(result);

        }
        public async Task<Produto> AtualizarProdutoAsync(Produto produto)
        {
            try
            {
                _context.Update(produto);
                await _context.SaveChangesAsync();
                return produto;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar categoria", ex);
            }
        }



        public async Task<Produto> ExcluirProdutoAsync(Produto? produto)
        {
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();
            return produto;
        }
    }
}
