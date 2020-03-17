using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Seventh.Bot.Resource.Entities;

namespace Seventh.Bot.Resource
{
    public class QBotDbContext : DbContext
    {

        public QBotDbContext(DbContextOptions<QBotDbContext> options) : base(options)
        {
        }

        public DbSet<AccountBinding> AccountBindings { get; set; }

        public DbSet<BoundAccount> BoundAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoundAccount>().HasKey(a => new { a.Pid,a.Qq } );
            modelBuilder.Entity<AccountBinding>().HasKey(a => new { a.BoundAccountPid, a.Qq });

            modelBuilder.Entity<AccountBinding>()
                .HasOne( b => b.BoundAccount)
                .WithOne( b => b.AccountBinding )
                .HasForeignKey<BoundAccount>(ab => new { ab.Pid, ab.Qq })
                .OnDelete( DeleteBehavior.Cascade );
        }
    }
}
