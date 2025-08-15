namespace Domain.Exceptions.SubCategoriaException
{
    public class SubCategoriaNotFoundException : Exception
    {
        private const string Mensagem = "Ops a subcategoria não foi encontrado";

        public SubCategoriaNotFoundException() : base(Mensagem) { }


        public SubCategoriaNotFoundException(string mensagem) : base(mensagem)
        {

        }

        public SubCategoriaNotFoundException(Exception innerException) : base(Mensagem, innerException)
        {

        }
    }
}
