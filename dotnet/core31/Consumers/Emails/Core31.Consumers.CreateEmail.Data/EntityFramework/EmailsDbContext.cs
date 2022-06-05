using Microsoft.EntityFrameworkCore;

namespace Core31.EventSubscribers.Emails.Data.EntityFramework
{
    public class EmailsDbContext : DbContext
    {
        public EmailsDbContext(DbContextOptions<EmailsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .HasPostgresExtension("pgcrypto");

            builder
                .Entity<Email>(emails =>
                {
                    emails
                        .HasKey(e => e.Id);

                    emails
                        .Property(e => e.Id)
                        .HasDefaultValueSql("gen_random_uuid()");

                    emails
                        .Property(e => e.Address)
                        .HasMaxLength(255);

                    emails
                        .HasIndex(e => e.Address)
                        .IsUnique();
                });
        }

        public DbSet<Email> Emails { get; set; }
    }
}
