using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;
using MediatR;

namespace Finances.Application.Abstractions.Users.Commands;
public sealed record CreateUserCurrencyCommand(Guid UserId, Guid CurrencyId) : IRequest;

public class CreateUserCurrencyHandler(IBaseRepository<UserCurrency> repository, IStateSaveChanges stateSaveChanges) : IRequestHandler<CreateUserCurrencyCommand>
{
    public async Task Handle(CreateUserCurrencyCommand request, CancellationToken cancellationToken)
    {
        await repository.CreateAsync(new() { UserId = request.UserId, CurrencyId = request.CurrencyId });
        await stateSaveChanges.SaveChangesAsync(cancellationToken);
    }
}