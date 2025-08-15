using Domain.Models;
using Domain.Repositorys;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace DataAccess.Repositorys
{
    public class SubCategoriaRepository : ISubCategoriaRepository
    {
        private readonly DatabaseContext _context;

        public SubCategoriaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> CriarSubCategoriaAsync(SubCategoria categoria)
        {
            try
            {
                using var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "dbo.SubCategoria_CadastrarSubCategoria";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Nome", categoria.Nome ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@Status", categoria.Status));
                command.Parameters.Add(new SqlParameter("@DataCriacao", categoria.DataCriacao));
                command.Parameters.Add(new SqlParameter("@DataAtualizacao", categoria.DataAtualizacao ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@CategoriaID", categoria.CategoriaId));

                if (command.Connection.State != ConnectionState.Open)
                    await command.Connection.OpenAsync();

                var result = await command.ExecuteScalarAsync(); // retorna apenas o valor da primeira coluna
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                // Logar ou lançar exceção customizada
                throw new Exception("Erro ao criar categoria", ex);
            }
        }

        public async Task<SubCategoria> AtualizarSubCategoriaAsync(SubCategoria categoria)
        {
            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
                return categoria;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar categoria", ex);
            }
        }

        public async Task<SubCategoria> ExcluirSubCategoriaAsync(SubCategoria categoria)
        {
            try
            {
                _context.Remove(categoria);
                await _context.SaveChangesAsync();
                return categoria;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir categoria", ex);
            }
        }


        public async Task<SubCategoria> BuscarSubCategoriaPorIdAsync(int id)
        {
            return await _context.SubCategorias
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<SubCategoria>> BuscarSubCategoriasAsync(int? ID)
        {
            try
            {
                var idParam = new SqlParameter("@ID", ID ?? (object)DBNull.Value);
                

                var resultado = await _context.SubCategorias
                    .FromSqlRaw(
                        "EXEC SubCategoria_BuscarSubCategoria @ID",
                        idParam)
                    .ToListAsync();

                return resultado;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<SubCategoria>> BuscarSubCategoriasPorIdAsync(int categoriaId)
        {
            return await _context.SubCategorias
                .Where(s => s.CategoriaId == categoriaId)
                .ToListAsync();
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
