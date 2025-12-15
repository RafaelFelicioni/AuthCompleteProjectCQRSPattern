using FluentValidation;

namespace CleanArchMonolit.Shared.Utils
{
    public static class CpfCnpjValidator
    {
        /// <summary>
        /// Valida TaxId: se até 11 dígitos, CPF; se exatamente 14, CNPJ.
        /// </summary>
        public static IRuleBuilderOptions<T, string> TaxId<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(ValidateTaxId)
                .WithMessage((rootObject, taxIdValue) =>
                {
                    var digits = new string(taxIdValue
                        .Where(char.IsDigit)
                        .ToArray());

                    if (digits.Length <= 11)
                        return "CPF inválido";

                    return "CNPJ inválido";
                });
        }

        private static bool ValidateTaxId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Extrai apenas os dígitos
            var digits = new string(value.Where(char.IsDigit).ToArray());
            if (digits.Length <= 11)
                return ValidateCpf(digits);
            if (digits.Length > 11)
                return ValidateCnpj(digits);

            return false;
        }

        private static bool ValidateCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            var digits = new string(cpf.Where(char.IsDigit).ToArray());
            if (digits.Length != 11)
                return false;

            var invalids = Enumerable.Range(0, 10).Select(i => new string(char.Parse(i.ToString()), 11));
            if (invalids.Contains(digits))
                return false;

            var numbers = digits.Select(c => int.Parse(c.ToString())).ToArray();
            // Primeiro dígito
            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += numbers[i] * (10 - i);
            int remainder = sum % 11;
            int checkDigit1 = remainder < 2 ? 0 : 11 - remainder;
            if (numbers[9] != checkDigit1)
                return false;

            // Segundo dígito
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += numbers[i] * (11 - i);
            remainder = sum % 11;
            int checkDigit2 = remainder < 2 ? 0 : 11 - remainder;
            if (numbers[10] != checkDigit2)
                return false;

            return true;
        }

        private static bool ValidateCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            var digits = new string(cnpj.Where(char.IsDigit).ToArray());
            if (digits.Length != 14)
                return false;

            var invalids = Enumerable.Range(0, 10).Select(i => new string(char.Parse(i.ToString()), 14));
            if (invalids.Contains(digits))
                return false;

            var numbers = digits.Select(c => int.Parse(c.ToString())).ToArray();
            int[] multipliers1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multipliers2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Primeiro dígito
            int sum = 0;
            for (int i = 0; i < 12; i++)
                sum += numbers[i] * multipliers1[i];
            int remainder = sum % 11;
            int checkDigit1 = remainder < 2 ? 0 : 11 - remainder;
            if (numbers[12] != checkDigit1)
                return false;

            // Segundo dígito
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += numbers[i] * multipliers2[i];
            remainder = sum % 11;
            int checkDigit2 = remainder < 2 ? 0 : 11 - remainder;
            if (numbers[13] != checkDigit2)
                return false;

            return true;
        }
    }
}
