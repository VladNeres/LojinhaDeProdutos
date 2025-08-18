using Domain.Dtos.ProdutoDtos;
using Domain.Dtos.SubCategoriaDtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mapper
{
    public static class ProdutoMapper
    {
        public static Produto ParaProduto(this ProdutoDto produtoDto) =>
        new Produto {
          Nome =  produtoDto.Nome,
          Descricao =  produtoDto.Descricao,
          DataCriacao =  DateTime.Now,
          Preco =   produtoDto.Preco,
          Status = produtoDto.Status,
          SubCategoriaId = produtoDto.SubCategoriaId,
        };

        public static void AtualizarComProdutoDto(this Produto produto, ProdutoDto dto)
        {
            produto.Nome = dto.Nome;
            produto.Descricao = dto.Descricao;
            produto.Preco = dto.Preco;
            produto.DataAtualizacao = DateTime.Now;
        }
    }
}
