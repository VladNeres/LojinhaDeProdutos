using ApplicationServices.Dtos;
using Dapper;
using Domain.Models;
using Domain.Repositorys;
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

        public async Task<Categoria> CriarCategoriaAsync(Categoria categoria)
        {
            try
            {
                await _context.AddAsync(categoria);
                await _context.SaveChangesAsync();
                return categoria;
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
            var idParam = new MySqlParameter("@ID", ID);
            var nomeParam = new MySqlParameter("@Nome", nome ?? (object)DBNull.Value);
            var statusParam = new MySqlParameter("@Status", status.HasValue ? status.Value : (object)DBNull.Value);
            var ordenarPorParam = new MySqlParameter("@OrdenarPor", ordenarPor ?? (object)DBNull.Value);
            var tipoOrdenacaoParam = new MySqlParameter("@TipoOrdenacao", tipoOrdenacao);

            var resultado = await _context.Categorias
                .FromSqlRaw("CALL Categoria_BuscarCategorias(@ID, @Nome, @Status, @OrdenarPor, @TipoOrdenacao)",
                    idParam, nomeParam, statusParam, ordenarPorParam, tipoOrdenacaoParam)
                .ToListAsync();

            return resultado;
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
