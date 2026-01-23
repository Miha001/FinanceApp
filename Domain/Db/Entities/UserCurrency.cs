namespace Finances.Domain.Db.Entities;
public class UserCurrency
{
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public Guid CurrencyId { get; set; }
    public virtual Currency Currencies { get; set; }
}