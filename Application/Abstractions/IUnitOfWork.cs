using Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.Abstractions;

/// <summary>
/// Позволяет реализовать транзакцию EF Core.
/// </summary>
public interface IUnitOfWork : IStateSaveChanges
{
    /// <summary>
    /// Создание транзакции.
    /// </summary>
    /// <returns></returns>
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default);
}