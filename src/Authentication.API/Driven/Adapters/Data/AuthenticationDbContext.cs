using Authentication.API.Models;
using KeyPairJWT.Core.Models;
using KeyPairJWT.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Authentication.API.Driven.Adapters.Data
{
    public sealed class AuthenticationDbContext : IdentityDbContext, ISecurityKeyContext
    {
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options)
            : base(options)
        {
        }

        public DbSet<KeyMaterial> SecurityKeys { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}