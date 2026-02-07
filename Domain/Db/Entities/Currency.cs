namespace Finances.Domain.Db.Entities;

public class Currency : IEntityId<Guid>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; private set; } = Guid.NewGuid();

    /// <summary>
    /// Название валюты
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Курс валюты к рублю
    /// </summary>
    public decimal Rate { get; private set; }

    public virtual ICollection<UserCurrency> UserCurrencies { get; private set; }

    public void UpdateRate(decimal newRate)
    {
        if (newRate <= 0)
            throw new ArgumentException("New rate must be positive.", nameof(newRate));

        if (Rate == newRate) return;

        Rate = newRate;
    }

    public Currency(string name, decimal rate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Currency name cannot be empty.", nameof(name));

        if (rate <= 0)
            throw new ArgumentException("Rate must be positive.", nameof(rate));

        Id = Guid.CreateVersion7();
        Name = name;
        Rate = rate;

        UserCurrencies = [];
    }

    private Currency() { }
}