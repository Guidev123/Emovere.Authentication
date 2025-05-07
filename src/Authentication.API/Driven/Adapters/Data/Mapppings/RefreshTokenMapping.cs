using Authentication.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.Driven.Adapters.Data.Mapppings;

public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserIdentification).IsRequired().HasColumnType("VARCHAR(160)");
    }
}