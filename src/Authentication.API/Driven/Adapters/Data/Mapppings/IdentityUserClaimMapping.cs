using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.Driven.Adapters.Data.Mapppings;

public class IdentityUserClaimMapping : IEntityTypeConfiguration<IdentityUserClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
    {
        builder.ToTable("Claim");
        builder.HasKey(uc => uc.Id);
        builder.Property(u => u.ClaimType).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.ClaimValue).HasColumnType("VARCHAR").HasMaxLength(160);
    }
}
