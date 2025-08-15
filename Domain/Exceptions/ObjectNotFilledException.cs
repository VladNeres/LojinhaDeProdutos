using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ObjectNotFilledException : Exception
    {
        private const string Mensagem = "Ops Parece que voce nao preencheu algum campo, por favor preencha os campos necessarios!";

        public ObjectNotFilledException(): base(Mensagem) {  }


        public ObjectNotFilledException(string mensagem) :base(mensagem)
        {
            
        }

        public ObjectNotFilledException(Exception innerException) : base(Mensagem, innerException)
        {
            
        }
    }
}
