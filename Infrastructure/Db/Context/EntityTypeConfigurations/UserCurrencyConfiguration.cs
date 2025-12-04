using Domain.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Db.Context.EntityTypeConfigurations;
public class UserCurrencyConfiguration : IEntityTypeConfiguration<UserCurrency>
{
    public void Configure(EntityTypeBuilder<UserCurrency> b)
    {
        b.ToTable("M2M_user_currencies");
        b.HasKey(x =>  new { x.UserId, x.CurrencyId });
    }
}