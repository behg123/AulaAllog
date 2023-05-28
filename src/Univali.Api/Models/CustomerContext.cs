using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Univali.Api.Entities;

namespace Univali.Api.Models
{
   public class CustomerContext : DbContext
   {
      public DbSet<Customer> Customers { get; set; }


      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
            // Remove pluralizing table name convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Configure the Customer entity
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id) // Set the primary key
                .Property(c => c.Name)
                .IsRequired(); // Set Name as required

            // Add more configurations if needed

            base.OnModelCreating(modelBuilder);
      }
   }
}