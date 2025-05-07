using Authentication.API.Extensions;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Authentication.API.Application.Commands.Users.Register
{
    public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FirstName)
                      .NotEmpty().WithMessage(ResponseMessages.FIRSTNAME_CANNOT_BE_EMPTY)
                      .Length(2, 50).WithMessage(ResponseMessages.FIRSTNAME_LENGTH_2_TO_50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(ResponseMessages.LASTNAME_CANNOT_BE_EMPTY)
                .Length(2, 50).WithMessage(ResponseMessages.LASTNAME_LENGTH_2_TO_50);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ResponseMessages.EMAIL_CANNOT_BE_EMPTY)
                .EmailAddress().WithMessage(ResponseMessages.INVALID_EMAIL_FORMAT);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ResponseMessages.PASSWORD_CAN_NOT_BE_EMPTY)
                .MinimumLength(8).WithMessage(ResponseMessages.PASSWORD_MIN_LENGTH)
                .Must(HasUpperCase).WithMessage(ResponseMessages.PASSWORD_UPPERCASE_REQUIRED)
                .Must(HasLowerCase).WithMessage(ResponseMessages.PASSWORD_LOWERCASE_REQUIRED)
                .Must(HasDigit).WithMessage(ResponseMessages.PASSWORD_DIGIT_REQUIRED)
                .Must(HasSpecialCharacter).WithMessage(ResponseMessages.PASSWORD_SPECIAL_REQUIRED);

            RuleFor(c => c.Document)
                .Must(DocumentIsValid)
                .WithMessage(ResponseMessages.INVALID_DOCUMENT);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage(ResponseMessages.PASSWORDS_DO_NOT_MATCH)
                .When(x => !string.IsNullOrEmpty(x.Password))
                .NotEmpty().WithMessage(ResponseMessages.PASSWORD_CAN_NOT_BE_EMPTY)
                .MinimumLength(8).WithMessage(ResponseMessages.PASSWORD_MIN_LENGTH);

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage(ResponseMessages.BIRTH_DATE_CANNOT_BE_EMPTY)
                .Must(IsValidAge).WithMessage(ResponseMessages.AGE_MUST_BE_OVER_16);
        }

        private static bool HasUpperCase(string password) => password.Any(char.IsUpper);

        private static bool DocumentIsValid(string cpf) => IsValidDocument(cpf);

        private static bool HasLowerCase(string password) => password.Any(char.IsLower);

        private static bool HasDigit(string password) => password.Any(char.IsDigit);

        private static bool IsValidAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age)) age--;

            return age >= MIN_AGE;
        }

        private static bool HasSpecialCharacter(string password)
        {
            var specialCharRegex = new Regex(@"[!@#$%^&*(),.?""{}|<>]");
            return specialCharRegex.IsMatch(password);
        }

        public static string JustNumbers(string input) => new(input.Where(char.IsDigit).ToArray());

        private const int CPF_MAX_LENGTH = 11;
        private const int MIN_AGE = 16;

        public static bool IsValidDocument(string number)
        {
            number = JustNumbers(number);

            if (number.Length > CPF_MAX_LENGTH)
                return false;

            while (number.Length != CPF_MAX_LENGTH)
                number = '0' + number;

            var equal = true;
            for (var i = 1; i < CPF_MAX_LENGTH && equal; i++)
                if (number[i] != number[0])
                    equal = false;

            if (equal || number == "12345678909")
                return false;

            var numbers = new int[CPF_MAX_LENGTH];

            for (var i = 0; i < CPF_MAX_LENGTH; i++)
                numbers[i] = int.Parse(number[i].ToString());

            var sum = 0;
            for (var i = 0; i < 9; i++)
                sum += (10 - i) * numbers[i];

            var result = sum % CPF_MAX_LENGTH;

            if (result == 1 || result == 0)
            {
                if (numbers[9] != 0)
                    return false;
            }
            else if (numbers[9] != CPF_MAX_LENGTH - result)
                return false;

            sum = 0;
            for (var i = 0; i < 10; i++)
                sum += (CPF_MAX_LENGTH - i) * numbers[i];

            result = sum % CPF_MAX_LENGTH;

            if (result == 1 || result == 0)
            {
                if (numbers[10] != 0)
                    return false;
            }
            else if (numbers[10] != CPF_MAX_LENGTH - result)
                return false;

            return true;
        }
    }
}