using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using static Dapper.SqlMapper;

namespace DataAccess.Common
{
    public  class BaseRepository
    {

        protected readonly string _connectionString;
        public Guid ConnectionID { get; private set; }
        private readonly bool _captureConnectionID = false;

        public BaseRepository(string connectionString, bool captureClientConnection = false) =>
              (_connectionString, _captureConnectionID) = (connectionString, captureClientConnection);


        protected  async Task<int> ExecuteAsync(string query, object param = null, CommandType? commandType = null)

        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            GetClientConnection(conn);
            return await conn.ExecuteAsync(query, param, commandType: commandType);

        }
        protected async Task<T> QueryFirstOrDefault<T>(string query, object param = null, CommandType? commandType = null)
        {
            try
            {
                using IDbConnection conn = new SqlConnection(_connectionString);
                conn.Open();

                GetClientConnection(conn);

                return await conn.QueryFirstOrDefaultAsync<T>(query, param, commandType: commandType);

            }
            catch (Exception)
            {

                throw;
            }
        }
        protected  async Task<T> QueryFirstOrDefaultAsync<T>(string query, object param = null, CommandType? commandType = null)

        {
            try
            {
                using IDbConnection conn = new SqlConnection(_connectionString);
                conn.Open();

                GetClientConnection(conn);

                return await conn.QueryFirstOrDefaultAsync<T>(query, param, commandType: commandType);

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected  async Task<T> QuerySingleAsync<T>(string query, object param = null, CommandType? commandType = null)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            GetClientConnection(conn);

            return await conn.QuerySingleAsync<T>(query, param, commandType: commandType);
        }



        protected async Task<IEnumerable<T>> QueryAsync<T>(string query, object param = null, CommandType? commandType = null)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            GetClientConnection(conn);

            return await conn.QueryAsync<T>(query, param, commandType: commandType);
        }

        protected async Task<T> MultipleQueryAsync<T>(string query, Func<GridReader, Task<T>> mapearRetorno, object param = null, CommandType? commandType = null)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            GetClientConnection(conn);

            using (var retorno = await conn.QueryMultipleAsync(query, param, commandType: commandType))
            {
                return await mapearRetorno(retorno);
            }
        }

        protected async Task<IEnumerable<T2>> QueryAsync<T, T1, T2>(string query, Func<T, T1, T2> map, object param = null, CommandType? commandType = null, string splitOn = "id")
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            GetClientConnection(conn);

            return await conn.QueryAsync(query, map, param: param, commandType: commandType, splitOn: splitOn);

        }

        protected async Task<IEnumerable<T3>> QueryAsync<T, T1, T2, T3>(string query, Func<T, T1, T2, T3> map, object param = null, CommandType? commandType = null, string splitOn = "id")
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            GetClientConnection(conn);

            return await conn.QueryAsync(query, map, param: param, commandType: commandType, splitOn: splitOn);

        }

        protected async Task<IEnumerable<T4>> QueryAsync<T, T1, T2, T3, T4>(string query, Func<T, T1, T2, T3, T4> map, object param = null, CommandType? commandType = null, string splitOn = "id")
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            conn.Open();
            GetClientConnection(conn);

            return await conn.QueryAsync(query, map, param: param, commandType: commandType, splitOn: splitOn);

        }

        private void GetClientConnection(IDbConnection conn)
        {
            if (_captureConnectionID)
                ConnectionID = (conn as SqlConnection).ClientConnectionId;
        }
    }
}
