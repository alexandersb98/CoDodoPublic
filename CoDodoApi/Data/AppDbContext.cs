using CoDodoApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}

    public DbSet<Process> Processes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Constraint: each unique combination of Process.Name and Process.FK_OpportunityUriForAssignment can only occur once in the database
        // Configure foreign key
        modelBuilder.Entity<Process>()
            .HasOne(p => p.Opportunity)
            .WithMany()
            .HasForeignKey(p => p.FK_OpportunityUriForAssignment)
            .HasPrincipalKey(o => o.UriForAssignment);

        // Configure unique index
        modelBuilder.Entity<Process>()
            .HasIndex(p => new { p.Name, p.FK_OpportunityUriForAssignment })
            .IsUnique();
    }
}