namespace Finances.Application.Abstractions.Shared;

/// <summary>
/// Интерфейс для реализации сохранения состояния сущностей.
/// </summary>
public interface IStateSaveChanges
{
    /// <summary>
    /// Сохранить все изменения сущностей.
    /// </summary>
    /// <param name="cancellationToken"></param>
    Task SaveChanges(CancellationToken cancellationToken = default);
}