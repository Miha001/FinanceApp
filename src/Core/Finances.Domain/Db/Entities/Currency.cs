namespace Finances.Domain.Db.Entities;

public class Currency : IEntityId<Guid>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; private set; }

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
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newRate, nameof(newRate));

        if (Rate == newRate) return;

        Rate = newRate;
    }

    public Currency(string name, decimal rate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rate, nameof(rate));

        Id = Guid.CreateVersion7();
        Name = name;
        Rate = rate;

        UserCurrencies = [];
    }

    private Currency() { }
}