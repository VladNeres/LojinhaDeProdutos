using Domain.Dtos.CategoriaDtos;
using Domain.Dtos.SubCategoriaDtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapper
{
    public static class SubCategoriaMapper
    {
        public static void AtualizarComSubDto(this SubCategoria categoria, SubCategoriaDto dto)
        {
            categoria.Nome = dto.Nome;
            categoria.DataAtualizacao = DateTime.Now;
        }
    }
}
