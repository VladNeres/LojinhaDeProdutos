namespace Domain.Models
{
    public class Categoria
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public bool Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao {  get; set; }
    }
}
