using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos.SubCategoriaDtos
{
    public class SubCategoriaDto
    {
          public string Nome { get; set; }
        public bool Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public int CategoriaId { get; set; }
        
    }
}
