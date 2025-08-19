using Domain.Models;
using Domain.Services;
using Domain.Repositorys;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Domain.Mapper;
using Domain.Dtos.CategoriaDtos;
using Domain.Exceptions;
using Domain.Exceptions.CategoriaException;
using FluentValidation;

namespace ApplicationServices.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ILogger<CategoriaService> _logger;
        private readonly IValidator<string> _validator;

        public CategoriaService(ICategoriaRepository categoriaRepository, ILogger<CategoriaService> logger, IValidator<string> validator)
        {
            _categoriaRepository = categoriaRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Categoria> CriarCategoria(string nomeCategoria)
        {
            _logger.LogInformation("Iniciando validação da categoria: {NomeCategoria}", nomeCategoria);

           var result =  _validator.Validate(nomeCategoria);

            if (!result.IsValid)
                throw new ObjectNotFilledException(string.Join("," , result.Errors.Select(e => e.ErrorMessage )));

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

        public async Task<Categoria> EditarCategoria(int ID,  CategoriaDto categoriaDto)
        {
            if (categoriaDto is null)
                throw new ObjectNotFilledException();


           Categoria categoriaExiste = await _categoriaRepository.BuscarCategoriaPorIdAsync(ID);


            if(categoriaExiste is null)
                throw new CategoriaNotFoundException();
          
            categoriaExiste.AtualizarComDto(categoriaDto);

            return await _categoriaRepository.AtualizarCategoriaAsync(categoriaExiste);

        }
        public async Task<Categoria> ExcluirCategoria(int Id)
        {
            if( Id <0)
                throw new ArgumentOutOfRangeException("Categoria Id  não encontrado");

            _logger.LogWarning("Realizando verificação se existe a categoria antes de exclui-la.");
            Categoria categoria =await  _categoriaRepository.BuscarCategoriaPorIdAsync(Id);

           return await _categoriaRepository.ExcluirCategoriaAsync(categoria);
        }
       

    }
}
