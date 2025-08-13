using ApplicationServices.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapper
{
    public static class CategoriaMapper
    {

            public static void AtualizarComDto(this Categoria categoria, CategoriaDto dto)
            {
                categoria.Nome = dto.Nome;
                categoria.DataAtualizacao = DateTime.Now;
            }

    }
}
