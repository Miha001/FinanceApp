using Finances.Application.Abstractions.Users;
using Finances.Application.Abstractions.Validators;
using Finances.Domain.Db.Entities;
using Finances.Domain.Enum;
using Finances.Domain.Models.Dto;
using Finances.Domain.Resources;
using Finances.Domain.Result;

namespace Finances.Application.Validations;

public class AuthValidator(IPasswordHasher passwordHasher) : IAuthValidator
{
    /// <inheritdoc/>
    public BaseResult ValidateLogin(User user, string enteredPassword)
    {
        if (user == null)
        {
            return BaseResult.Failure((int)ErrorCodes.UserNotFound, ErrorMessages.UserNotFound);
        }

        bool verified = passwordHasher.Verify(enteredPassword, passwordHash: user.Password);

        if (!verified)
        {
            return BaseResult.Failure((int)ErrorCodes.PasswordIsWrong, ErrorMessages.PasswordIsWrong);
        }

        return BaseResult.Success();
    }

    /// <inheritdoc/>
    public BaseResult ValidateRegister(User user, string enteredPassword, string enteredPasswordConfirm)
    {
        if (enteredPassword != enteredPasswordConfirm)
        {
            return DataResult<UserDto>.Failure((int)ErrorCodes.PasswordNotEqualsPasswordConfirm, ErrorMessages.PasswordNotEqualsPasswordConfirm);
        }

        if (user != null)
        {
            return DataResult<UserDto>.Failure((int)ErrorCodes.UserAlreadyExists, ErrorMessages.UserAlreadyExists);
        }

        return BaseResult.Success();
    }
}