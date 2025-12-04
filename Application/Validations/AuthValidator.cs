using Application.Abstractions.Helpers;
using Application.Abstractions.Validators;
using Application.Resources;
using Domain.Db.Entities;
using Domain.Enum;
using Domain.Models.Dto;
using Domain.Result;

namespace Application.Validations;

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