

namespace Domain.Dtos.ProdutoDtos
{
    public class ProdutoDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }
        public Decimal Preco { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int SubCategoriaId { get; set; }
    }
}
