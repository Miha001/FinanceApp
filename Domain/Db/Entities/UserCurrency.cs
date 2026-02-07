namespace Finances.Domain.Db.Entities;
public class UserCurrency
{
    public Guid UserId { get; private set; }
    public virtual User User { get; private set; }
    public Guid CurrencyId { get; private set; }
    public virtual Currency Currencies { get; private set; }

    public UserCurrency(Guid userId, Guid currencyId)
    {
        UserId = userId;
        CurrencyId = currencyId;
    }
}