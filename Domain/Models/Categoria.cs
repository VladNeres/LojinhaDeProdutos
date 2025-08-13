using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class Categoria
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public bool Status { get; set; }
        [JsonIgnore]
        public virtual List<SubCategoria> SubCategoria { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao {  get; set; }
    }
}
