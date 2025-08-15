namespace Domain.Exceptions.CategoriaException
{
    public class CategoriaNotFoundException : Exception
    {
        private const string Mensagem = "Ops a categoria não foi encontrado";

        public CategoriaNotFoundException() : base(Mensagem) { }


        public CategoriaNotFoundException(string mensagem) : base(mensagem)
        {

        }

        public CategoriaNotFoundException(Exception innerException) : base(Mensagem, innerException)
        {

        }
    }
}
