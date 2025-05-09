﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.API.Driven.Adapters.Data.Mapppings;

public class IdentityRoleClaimMapping : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {
        builder.ToTable("RoleClaim");
        builder.HasKey(rc => rc.Id);
        builder.Property(u => u.ClaimType).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.RoleId).HasColumnType("VARCHAR").HasMaxLength(160);
        builder.Property(u => u.ClaimValue).HasColumnType("VARCHAR").HasMaxLength(160);
    }
}
