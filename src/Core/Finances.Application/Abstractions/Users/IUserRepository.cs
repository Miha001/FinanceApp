using Finances.Application.Abstractions.Shared;
using Finances.Domain.Db.Entities;

namespace Finances.Application.Abstractions.Users;

public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Получить пользователя по имени
    /// </summary>
    Task<User?> GetByName(string name, CancellationToken ct = default);

    /// <summary>
    /// Проверить существует ли пользователь по имени
    /// </summary>
    Task<bool> ExistsByName(string name, CancellationToken ct = default);
}