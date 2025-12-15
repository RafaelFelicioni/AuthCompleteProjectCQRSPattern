using CleanArchMonolit.Application.Auth.DTO;
using CleanArchMonolit.Application.Auth.Login.Commands.Login;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CleanArchMonolit.Application.Auth.Login.Commands.Login.LoginCommandHandler;

namespace CleanArchMonolit.Application.Auth.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres.")
                .Must(ContainUppercase).WithMessage("Senha deve conter ao menos uma letra maiúscula.")
                .Must(ContainNumber).WithMessage("Senha deve conter ao menos um número.")
                .Must(ContainSpecialChar).WithMessage("Senha deve conter ao menos um caractere especial.");
        }

        private bool ContainUppercase(string password)
            => password.Any(char.IsUpper);

        private bool ContainNumber(string password)
            => password.Any(char.IsDigit);

        private bool ContainSpecialChar(string password)
            => Regex.IsMatch(password, @"[\W_]");
    }
}
