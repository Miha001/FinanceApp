using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using Finances.Domain.Enum;
using Finances.Domain.Resources;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Users.Commands;
public sealed record CreateUserCurrencyCommand(Guid CurrencyId) : IRequest<DataResult<bool>>;

public class CreateUserCurrencyHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<UserCurrency> repo,
    IStateSaveChanges stateSaveChanges) : IRequestHandler<CreateUserCurrencyCommand, DataResult<bool>>
{
    public async Task<DataResult<bool>> Handle(CreateUserCurrencyCommand request, CancellationToken ct)
    {
        var userId = currentUserProvider.UserId;

        var isExist = await repo.Exists(x => x.UserId == userId && x.CurrencyId == request.CurrencyId, ct);

        if (isExist)
        {
            return DataResult<bool>.Failure(
                (int)ErrorCodes.CurrencyAlreadyInFavorites,
                ErrorMessages.CurrencyAlreadyInFavorites);
        }

        await repo.Create(new(userId, request.CurrencyId), ct);
        await stateSaveChanges.SaveChanges(ct);

        return DataResult<bool>.Success(true);
    }
}