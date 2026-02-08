using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using Finances.Domain.Result;
using MediatR;

namespace Finances.Application.Abstractions.Users.Commands;
public sealed record CreateUserCurrencyCommand(Guid CurrencyId) : IRequest<DataResult<bool>>;

public class CreateUserCurrencyHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<UserCurrency> repository,
    IStateSaveChanges stateSaveChanges) : IRequestHandler<CreateUserCurrencyCommand, DataResult<bool>>
{
    public async Task<DataResult<bool>> Handle(CreateUserCurrencyCommand request, CancellationToken ct)
    {
        var userId = currentUserProvider.UserId;

        await repository.Create(new(userId, request.CurrencyId), ct);
        await stateSaveChanges.SaveChanges(ct);

        return DataResult<bool>.Success(true);
    }
}