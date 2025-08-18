using Castle.Components.DictionaryAdapter.Xml;
using Domain.Dtos.CategoriaDtos;
using FluentValidation;

namespace AplicacaoProjeto.Validators
{
    public class NameValidator : AbstractValidator<string>
    {
        public NameValidator()
        {
            RuleFor(nome => nome)
                .NotEmpty().WithMessage("O nome é obrigatório");
            RuleFor(nome => nome).Length(3, 250)
                .WithMessage("O nome precisa ter de 3 a 250 caracteres");
            RuleFor(nome => nome).Matches(@"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$")
                .WithMessage("Por favor o preencha o nome apenas com letras sem acentos");
        }
    }
}
