using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // define EmployeeId as primary and foreign key on Compensation
            modelBuilder.Entity<Compensation>()
                .HasKey(c => c.EmployeeId);

            modelBuilder.Entity<Compensation>()
                .HasOne(c => c.Employee)
                .WithOne(e => e.Compensation)
                .HasForeignKey<Compensation>(c => c.EmployeeId);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Compensation> Compensation { get; set; }
    }
}
