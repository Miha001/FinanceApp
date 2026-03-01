namespace Finances.Domain.Db.Entities;

public class User : IEntityId<Guid>
{
    private User() { }

    public User(string name, string passwordHash)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        ArgumentNullException.ThrowIfNullOrWhiteSpace(passwordHash, nameof(passwordHash));

        Id = Guid.CreateVersion7();
        Name = name;
        Password = passwordHash;
        UserCurrencies = [];
    }

    /// <inheritdoc/>
    public Guid Id { get; private set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; private set; }

    public virtual ICollection<UserCurrency> UserCurrencies { get; private set; }
}