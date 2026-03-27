using Microsoft.EntityFrameworkCore;
using RequisitionManagement.API.Models;

namespace RequisitionManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Requisition> Requisitions { get; set; }
        public DbSet<Approval> Approvals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Requisition>()
                .HasOne(r => r.Creator)
                .WithMany()
                .HasForeignKey(r => r.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Approval>()
                .HasOne(a => a.Approver)
                .WithMany()
                .HasForeignKey(a => a.ApproverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Approval>()
                .HasOne(a => a.Requisition)
                .WithMany()
                .HasForeignKey(a => a.RequisitionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}