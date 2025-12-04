namespace Domain.Db.Entities;

/// <summary>
/// Refresh-Токен пользователя для аутентификации и авторизации.
/// Нужен для обновления Access-токена.
/// </summary>
public class UserToken : IEntityId<Guid>
{
    ///<inheritdoc/>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Refresh-токен в виде строки
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// Дата и время, после которой токен будет недействительный.
    /// И его нужно будет обновить
    /// </summary>
    public DateTime RefreshTokenExpireTime { get; set; }

    /// <summary>
    /// Идентификатор пользователя, владеющего данным токеном
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь токена
    /// </summary>
    public User User { get; set; }
}