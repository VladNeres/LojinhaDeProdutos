using DataAccess.Repositorys;
using Domain.Dtos.CategoriaDtos;
using Domain.Dtos.SubCategoriaDtos;
using Domain.Exceptions;
using Domain.Exceptions.SubCategoriaException;
using Domain.Mapper;
using Domain.Models;
using Domain.Repositorys;
using Domain.Services;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;


namespace ApplicationServices.Services
{
    public class SubCategoriaService : ISubCategoriaService
    {
        private readonly ISubCategoriaRepository _subCategoriaRepository;
        private readonly ILogger<SubCategoriaService> _logger;
        private readonly ICategoriaRepository _categoriaRepository;

        public SubCategoriaService(ISubCategoriaRepository subCategoriaRepository, ILogger<SubCategoriaService> logger, ICategoriaRepository categoriaRepository)
        {
            _subCategoriaRepository = subCategoriaRepository;
            _logger = logger;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<SubCategoria> CriarSubCategoria(SubCategoriaDto subCategoria)
        {
            _logger.LogInformation("Iniciando validação da categoria: {NomeSubCategoria}", subCategoria);

            Categoria? categoria = await _categoriaRepository.BuscarCategoriaPorIdAsync(subCategoria.CategoriaId);

            if (subCategoria is null || categoria is null)
                throw new ArgumentNullException("Dados nao preenchidos, por gentileza insira um nome e uma categoria valida");

            ValidarSubCategoria(subCategoria.Nome);

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
                throw new ArgumentOutOfRangeException("Categoria Id  não encontrado");

            _logger.LogWarning("Realizando verificação se existe a categoria antes de exclui-la.");
            SubCategoria? subCategoria = await _subCategoriaRepository.BuscarSubCategoriaPorIdAsync(Id);

            if (subCategoria is null)
                throw new ArgumentNullException("SubCategoria nao encontrada");

            return await _subCategoriaRepository.ExcluirSubCategoriaAsync(subCategoria);
        }
        private void ValidarSubCategoria(string nomeCategoria)
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
