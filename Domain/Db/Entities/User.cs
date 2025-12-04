namespace Domain.Db.Entities;
public class User : IEntityId<Guid>
{
    /// <inheritdoc/>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Refresh-токен пользователя.
    /// </summary>
    public UserToken UserToken { get; set; }

    public virtual ICollection<UserCurrency> UserCurrencies { get; set; }
}