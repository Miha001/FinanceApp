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

    public virtual ICollection<UserCurrency> UserCurrencies { get; set; }
}