namespace Finances.Application.Abstractions.Shared;

public interface ICurrentUserProvider
{
    Guid UserId { get; }
}