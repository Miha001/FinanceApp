using Application.Abstractions.Repositories;
using Domain.Db.Entities;
using MediatR;

namespace Application.CQRS.Commands;
public sealed record CreateUserCurrencyCommand(Guid UserId, Guid CurrencyId) : IRequest;

public class CreateUserCurrencyHandler(IBaseRepository<UserCurrency> repository) : IRequestHandler<CreateUserCurrencyCommand>
{
    public async Task Handle(CreateUserCurrencyCommand request, CancellationToken cancellationToken)
    {
        await repository.CreateAsync(new() { UserId = request.UserId, CurrencyId = request.CurrencyId });
        await repository.SaveChangesAsync(cancellationToken);
    }
}