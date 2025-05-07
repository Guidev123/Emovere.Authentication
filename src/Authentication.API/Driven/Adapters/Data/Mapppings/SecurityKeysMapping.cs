﻿using KeyPairJWT.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.Driven.Adapters.Data.Mapppings;

public class SecurityKeysMapping : IEntityTypeConfiguration<KeyMaterial>
{
    public void Configure(EntityTypeBuilder<KeyMaterial> builder)
    {
        builder.ToTable("SecurityKeys");
        builder.HasKey(k => k.Id);
        builder.Property(k => k.KeyId).HasColumnType("VARCHAR").HasMaxLength(200);
        builder.Property(k => k.Type).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(k => k.Parameters).HasColumnType("NVARCHAR").HasMaxLength(3000);
        builder.Property(k => k.RevokedReason).HasColumnType("VARCHAR").HasMaxLength(160);
    }
}
