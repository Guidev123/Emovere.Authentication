using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.Driven.Adapters.Data.Mapppings;

public class IdentityUserRoleMapping : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.ToTable("UserRole");
        builder.HasKey(r => new { r.UserId, r.RoleId });
        builder.Property(x => x.UserId).HasColumnType("NVARCHAR").HasMaxLength(450);
        builder.Property(x => x.RoleId).HasColumnType("VARCHAR").HasMaxLength(160);
    }
}
