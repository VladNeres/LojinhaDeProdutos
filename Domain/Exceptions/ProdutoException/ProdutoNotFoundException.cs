using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.ProdutoException
{
    public class ProdutoNotFoundException : Exception
    {
        private const string Mensagem = "Ops o produto não foi encontrado";

        public ProdutoNotFoundException() : base(Mensagem) { }


        public ProdutoNotFoundException(string mensagem) : base(mensagem)
        {

        }

        public ProdutoNotFoundException(Exception innerException) : base(Mensagem, innerException)
        {

        }
    }
}
