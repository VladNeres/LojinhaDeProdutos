using Dapper;
using DataAccess.Common;
using Domain.Models;
using Domain.Repositorys;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DataAccess.Repositorys
{
    public class CategoriaRepository : BaseRepository, ICategoriaRepository
    {
       
        public CategoriaRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<int> CadastrarCategoria(Categoria categoria)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Nome", categoria.Nome);
            parametros.Add("@Status", categoria.Status);
            parametros.Add("@DataCriacao", categoria.DataCriacao);
            parametros.Add("@DataAtualizacao", categoria.DataAtualizacao);

            return await ExecuteAsync("Categoria_CadastrarCategoria", parametros, commandType: CommandType.StoredProcedure);

          
        }

        public async Task<IEnumerable<Categoria>> BuscarCategorias(int? ID, string? nome, bool? status, string? ordenarPor, string ordenacao)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@ID", ID);
            parametros.Add("@Nome", nome);
            parametros.Add("@Status", status);
            parametros.Add("@Ordenar_Por", ordenarPor);
            parametros.Add("@Ordenacao", ordenacao);

            return await QueryAsync<Categoria>("Categoria_BuscarCategorias",parametros, commandType: CommandType.StoredProcedure);
        }

        
    }
}
