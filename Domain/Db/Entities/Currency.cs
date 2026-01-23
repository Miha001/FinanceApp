namespace Finances.Domain.Db.Entities;
public class Currency : IEntityId<Guid>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Название валюты
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Курс валюты к рублю
    /// </summary>
    public decimal Rate { get; set; }

    public virtual ICollection<UserCurrency> UserCurrencies { get; set; }
}