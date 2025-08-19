using DataAccess.Repositorys;
using Domain.Dtos.CategoriaDtos;
using Domain.Dtos.SubCategoriaDtos;
using Domain.Exceptions;
using Domain.Exceptions.SubCategoriaException;
using Domain.Mapper;
using Domain.Models;
using Domain.Repositorys;
using Domain.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System.Text.RegularExpressions;


namespace ApplicationServices.Services
{
    public class SubCategoriaService : ISubCategoriaService
    {
        private readonly ISubCategoriaRepository _subCategoriaRepository;
        private readonly ILogger<SubCategoriaService> _logger;
        private readonly ICategoriaRepository _categoriaRepository;
        private IValidator<string> _validator;
        public SubCategoriaService(ISubCategoriaRepository subCategoriaRepository, ILogger<SubCategoriaService> logger, ICategoriaRepository categoriaRepository, IValidator<string> validator)
        {
            _subCategoriaRepository = subCategoriaRepository;
            _logger = logger;
            _categoriaRepository = categoriaRepository;
            _validator = validator;
        }

        public async Task<SubCategoria> CriarSubCategoria(SubCategoriaDto subCategoria)
        {
            _logger.LogInformation("Iniciando validação da subcategoria: {NomeSubCategoria}", subCategoria);

            Categoria? categoria = await _categoriaRepository.BuscarCategoriaPorIdAsync(subCategoria.CategoriaId);

            if (subCategoria is null || categoria is null)
                throw new ArgumentNullException("Dados nao preenchidos, por gentileza insira um nome e uma subcategoria valida");

            var result  =  _validator.Validate(subCategoria.Nome);

            if (!result.IsValid)
                throw new ObjectNotFilledException(string.Join(",", result.Errors.Select(e => e.ErrorMessage)));
            
            _logger.LogInformation("Validação concluída. Preparando objeto SubCategoria.");

            var subcategoria = new SubCategoria
            {
                Nome = subCategoria.Nome,
                Status = true,
                DataCriacao = DateTime.Now.ToLocalTime(),
                DataAtualizacao = null,
                CategoriaId = subCategoria.CategoriaId
            };

            _logger.LogInformation("Salvando Subcategoria no repositório.");

            await _subCategoriaRepository.CriarSubCategoriaAsync(subcategoria);

            _logger.LogInformation("Categoria salva com sucesso.");

            return subcategoria;
        }

        public async Task<IEnumerable<SubCategoria>> BuscarSubCategorias(int? ID)
        {
            _logger.LogInformation("Iniciando busca de categorias. ID: {ID}",
                ID);

            var resultado = await _subCategoriaRepository.BuscarSubCategoriasAsync(ID);

            if (resultado is null)
                throw new SubCategoriaNotFoundException();

            _logger.LogInformation("Busca concluída. Total encontrado: {Quantidade}", resultado.ToList().Count);

            return resultado;
        }

        public async Task<SubCategoria> EditarSubCategoria(int ID, SubCategoriaDto subCategoriaDto)
        {
            if (subCategoriaDto is null)
                throw new ObjectNotFilledException();


            var subCategoriaExiste = await _subCategoriaRepository.BuscarSubCategoriaPorIdAsync(ID);
            var isCategoriaExiste = await _categoriaRepository.BuscarCategoriaPorIdAsync(subCategoriaDto.CategoriaId);

            if (subCategoriaExiste is null  )
                throw new SubCategoriaNotFoundException();
            
            if( isCategoriaExiste is null)
                throw new SubCategoriaNotFoundException();
            subCategoriaExiste.AtualizarComSubDto(subCategoriaDto);
            var resultado = await _subCategoriaRepository.AtualizarSubCategoriaAsync(subCategoriaExiste);

            return resultado;

        }
        public async Task<SubCategoria> ExcluirSubCategoria(int Id)
        {
            if (Id < 0)
                throw new ArgumentOutOfRangeException("SubCategoria Id  não encontrado");

            _logger.LogWarning("Realizando verificação se existe a subcategoria antes de exclui-la.");
            SubCategoria? subCategoria = await _subCategoriaRepository.BuscarSubCategoriaPorIdAsync(Id);

            if (subCategoria is null)
                throw new ArgumentNullException("SubCategoria nao encontrada");

            return await _subCategoriaRepository.ExcluirSubCategoriaAsync(subCategoria);
        }
    }
}
