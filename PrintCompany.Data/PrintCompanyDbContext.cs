using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrintCompany.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintCompany.Data
{
    public class PrintCompanyDbContext : IdentityDbContext<IdentityUser>
    {
        public PrintCompanyDbContext(DbContextOptions<PrintCompanyDbContext> contextOptions) 
            : base(contextOptions)
        {
        }
        public PrintCompanyDbContext()
        {
        }

        public DbSet<Order> Orders { get; set; }        
        public DbSet<ItemColor> ItemColors { get; set; }
        public DbSet<ItemSize> ItemSizes { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    // Customize the ASP.NET Identity model and override the defaults if needed.
        //    // For example, you can rename the ASP.NET Identity table names and more.
        //    // Add your customizations after calling base.OnModelCreating(builder);
        //}


        //public override int SaveChanges()
        //{
        //    AddTimestamps();
        //    return base.SaveChanges();
        //}

        //public override async Task<int> SaveChangesAsync()
        //{
        //    AddTimestamps();
        //    return await base.SaveChangesAsync();
        //}

        //private void AddTimestamps()
        //{
        //    var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        //    var currentUsername = !string.IsNullOrEmpty(System.Web.HttpContext.Current?.User?.Identity?.Name)
        //        ? HttpContext.Current.User.Identity.Name
        //        : "Anonymous";

        //    foreach (var entity in entities)
        //    {
        //        if (entity.State == EntityState.Added)
        //        {
        //            ((BaseEntity)entity.Entity).DateCreated = DateTime.UtcNow;
        //            ((BaseEntity)entity.Entity).UserCreated = currentUsername;
        //        }

        //        ((BaseEntity)entity.Entity).DateModified = DateTime.UtcNow;
        //        ((BaseEntity)entity.Entity).UserModified = currentUsername;
        //    }
        //}


    }
}
