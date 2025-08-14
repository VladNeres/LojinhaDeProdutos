using DataAccess.Repositorys;
using Domain.Dtos.CategoriaDtos;
using Domain.Dtos.SubCategoriaDtos;
using Domain.Mapper;
using Domain.Models;
using Domain.Repositorys;
using Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplicationServices.Services
{
    public class SubCategoriaService : ISubCategoriaService
    {
        private readonly ISubCategoriaRepository _subCategoriaRepository;
        private readonly ILogger<SubCategoriaService> _logger;

        public SubCategoriaService(ISubCategoriaRepository subCategoriaRepository, ILogger<SubCategoriaService> logger)
        {
            _subCategoriaRepository = subCategoriaRepository;
            _logger = logger;
        }

        public async Task<SubCategoria> CriarSubCategoria(SubCategoria subCategoria)
        {
            _logger.LogInformation("Iniciando validação da categoria: {NomeCategoria}", subCategoria);

            ValidarSubCategoria(subCategoria.Nome);

            _logger.LogInformation("Validação concluída. Preparando objeto Categoria.");

            var categoria = new SubCategoria
            {
                Nome = subCategoria.Nome,
                Status = true,
                DataCriacao = DateTime.Now.ToLocalTime(),
                DataAtualizacao = null
            };

            _logger.LogInformation("Salvando categoria no repositório.");

            await _subCategoriaRepository.CriarSubCategoriaAsync(categoria);

            _logger.LogInformation("Categoria salva com sucesso.");

            return categoria;
        }

        public async Task<IEnumerable<SubCategoria>> BuscarSubCategorias(int? ID, string? nome, bool? status, string? ordenarPor, string tipoOrdenacao)
        {
            _logger.LogInformation("Iniciando busca de categorias. ID: {ID}, Nome: {Nome}, Status: {Status}, OrdenarPor: {OrdenarPor}, Ordenacao: {Ordenacao}",
                ID, nome, status, ordenarPor, tipoOrdenacao);

            if (tipoOrdenacao == null || tipoOrdenacao.ToUpper() != "ASC" && tipoOrdenacao.ToUpper() != "DESC")
            {
                _logger.LogWarning("Parâmetro de ordenação inválido: {Ordenacao}", tipoOrdenacao);
                throw new ArgumentException("O parâmetro 'ordenacao' deve ser 'ASC' ou 'DESC'.");
            }
            string campoOrdenacao = string.IsNullOrEmpty(ordenarPor) ? "ID" : ordenarPor;

            var resultado = await _subCategoriaRepository.BuscarSubCategoriasAsync(ID, nome, status, ordenarPor, tipoOrdenacao);

            _logger.LogInformation("Busca concluída. Total encontrado: {Quantidade}", resultado.ToList().Count);

            return resultado;
        }

        public async Task<SubCategoria> EditarSubCategoria(int ID, SubCategoriaDto categoriaDto)
        {
            if (categoriaDto is null)
                throw new ArgumentNullException("A categoria não pode estar vazia ou nula.");


            SubCategoria subCategoriaExiste = await _subCategoriaRepository.BuscarSubCategoriaPorIdAsync(ID);


            if (subCategoriaExiste is null)
                throw new ArgumentNullException("A categoria não encontrada.");

            subCategoriaExiste.AtualizarComSubDto(categoriaDto);

            var resultado = await _subCategoriaRepository.AtualizarSubCategoriaAsync(subCategoriaExiste);

            return resultado;

        }
        public async Task<SubCategoria> ExcluirSubCategoria(int Id)
        {
            if (Id < 0)
                throw new ArgumentOutOfRangeException("Categoria Id  não encontrado");

            _logger.LogWarning("Realizando verificação se existe a categoria antes de exclui-la.");
            SubCategoria subCategoria = await _subCategoriaRepository.BuscarSubCategoriaPorIdAsync(Id);

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
