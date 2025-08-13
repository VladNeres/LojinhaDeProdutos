using Domain.Models;
using Domain.Services;
using Domain.Repositorys;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ApplicationServices.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<CategoriaService> _logger;

        public CategoriaService(ICategoriaRepository categoriaRepository, ILogger<CategoriaService> logger)
        {
            _categoriaRepository = categoriaRepository;
            _logger = logger;
        }

        public async Task<Categoria> CriarCategoria(string nomeCategoria)
        {
            _logger.LogInformation("Iniciando validação da categoria: {NomeCategoria}", nomeCategoria);

            ValidarCategoria(nomeCategoria);

            _logger.LogInformation("Validação concluída. Preparando objeto Categoria.");

            var categoria = new Categoria
            {
                Nome = nomeCategoria,
                Status = true,
                DataCriacao = DateTime.Now.ToLocalTime(),
                DataAtualizacao = null
            };

            _logger.LogInformation("Salvando categoria no repositório.");

             await _categoriaRepository.CriarCategoriaAsync(categoria);

            _logger.LogInformation("Categoria salva com sucesso.");

            return categoria;
        }

        public async Task<IEnumerable<Categoria>> BuscarCategorias(int? ID, string? nome, bool? status, string? ordenarPor, string tipoOrdenacao)
        {
            _logger.LogInformation("Iniciando busca de categorias. ID: {ID}, Nome: {Nome}, Status: {Status}, OrdenarPor: {OrdenarPor}, Ordenacao: {Ordenacao}",
                ID, nome, status, ordenarPor, tipoOrdenacao);

            if (tipoOrdenacao == null || tipoOrdenacao.ToUpper() != "ASC" && tipoOrdenacao.ToUpper() != "DESC")
            {
                _logger.LogWarning("Parâmetro de ordenação inválido: {Ordenacao}", tipoOrdenacao);
                throw new ArgumentException("O parâmetro 'ordenacao' deve ser 'ASC' ou 'DESC'.");
            }

            string campoOrdenacao = string.IsNullOrEmpty(ordenarPor) ? "ID" : ordenarPor;

            var resultado = await _categoriaRepository.BuscarCategoriasAsync(ID, nome,status,  ordenarPor,tipoOrdenacao);

            _logger.LogInformation("Busca concluída. Total encontrado: {Quantidade}", resultado.ToList().Count);

            return resultado;
        }

        public async Task<Categoria> EditarCategoria(int ID, Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException("A categoria não pode estar vazia ou nula.");

           var categoriaExiste = await _categoriaRepository.BuscarCategoriaPorIdAsync(ID);

            if(categoriaExiste is null)
                throw new ArgumentNullException("A categoria não encontrada.");

            var resultado = await _categoriaRepository.AtualizarCategoriaAsync(categoria);

            return resultado;

        }
        private void ValidarCategoria(string nomeCategoria)
        {
            if (string.IsNullOrWhiteSpace(nomeCategoria))
            {
                _logger.LogWarning("Validação falhou: nome da categoria vazio ou em branco.");
                throw new ArgumentException("O nome é obrigatório.");
            }

            if (nomeCategoria.Length > 150)
            {
                _logger.LogWarning("Validação falhou: nome da categoria excede 150 caracteres.");
                throw new ArgumentException("O nome não pode ter mais que 150 caracteres.");
            }

            if (!Regex.IsMatch(nomeCategoria, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$"))
            {
                _logger.LogWarning("Validação falhou: nome da categoria contém caracteres inválidos.");
                throw new ArgumentException("O nome não pode conter símbolos. Apenas letras e acentuação são permitidas.");
            }

            _logger.LogInformation("Validação da categoria concluída com sucesso.");
        }

    }
}
