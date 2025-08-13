using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidObjectException : Exception
    {
        private const string Mensagem = "Ops Parece que voce nao preencheu algum campo, por favor preencha os campos necessarios!";

        public InvalidObjectException(): base(Mensagem) {  }


        public InvalidObjectException(string mensagem) :base(mensagem)
        {
            
        }

        public InvalidObjectException(Exception innerException) : base(Mensagem, innerException)
        {
            
        }
    }
}
