using ApplicationServices.Dtos;
using Dapper;
using Domain.Models;
using Domain.Repositorys;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataAccess.Repositorys
{
    public class CategoriaRepository :  ICategoriaRepository
    {
        private readonly DatabaseContext _context;

        public CategoriaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<int> CriarCategoriaAsync(Categoria categoria)
        {
            try
            {
                var nomeParam = new SqlParameter("@Nome", categoria.Nome ?? (object)DBNull.Value);
                var statusParam = new SqlParameter("@Status", categoria.Status);
                var dataCriacaoParam = new SqlParameter("@DataCriacao", categoria.DataCriacao);
                var dataAtualizacaoParam = new SqlParameter("@DataAtualizacao",
                    categoria.DataAtualizacao ?? (object)DBNull.Value);

                // Executa a procedure, retornando o ID inserido
                var resultado = await _context.Categorias
                    .FromSqlRaw(
                        "EXEC dbo.Categoria_CadastrarCategoria @Nome, @Status, @DataCriacao, @DataAtualizacao",
                        nomeParam, statusParam, dataCriacaoParam, dataAtualizacaoParam)
                    .Select(c => c.ID) // Ajuste se o SELECT da procedure retorna outro nome
                    .FirstOrDefaultAsync();

                return resultado;
            }
            catch (Exception ex)
            {
                // Logar ou lançar exceção customizada
                throw new Exception("Erro ao criar categoria", ex);
            }
        }

        public async Task<Categoria> AtualizarCategoriaAsync(Categoria categoria)
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

        public async Task<Categoria> ExcluirCategoriaAsync(Categoria categoria)
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

        public async Task<Categoria> BuscarNomeCategoriaAsync(string nome)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.Nome.ToUpper() == nome.ToUpper());
        }

        public async Task<Categoria> BuscarCategoriaPorIdAsync(int id)
        {
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<IEnumerable<Categoria>> BuscarCategoriasAsync(int? ID, string? nome, bool? status, string? ordenarPor, string tipoOrdenacao)
        {
            try
            {
                var idParam = new SqlParameter("@ID", ID ?? (object)DBNull.Value);
                var nomeParam = new SqlParameter("@Nome", nome ?? (object)DBNull.Value);
                var statusParam = new SqlParameter("@Status", status ?? (object)DBNull.Value);
                var ordenarPorParam = new SqlParameter("@OrdenarPor", ordenarPor ?? (object)DBNull.Value);
                var tipoOrdenacaoParam = new SqlParameter("@TipoOrdenacao", tipoOrdenacao ?? (object)DBNull.Value);

                var resultado = await _context.Categorias
                    .FromSqlRaw(
                        "EXEC Categoria_BuscarCategorias @ID, @Nome, @Status, @OrdenarPor, @TipoOrdenacao",
                        idParam, nomeParam, statusParam, ordenarPorParam, tipoOrdenacaoParam)
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
