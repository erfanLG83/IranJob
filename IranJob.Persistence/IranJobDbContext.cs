using IranJob.Domain.Auth;
using IranJob.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IranJob.Persistence
{
    public class IranJobDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<JobRequest> JobRequests { get; set; }
        public DbSet<Province> Provinces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER2016;Database=erfanla1_IranJobDB;User ID=erfanla1_biglg;password=$n9s44wZ;Trusted_Connection=True;MultipleActiveResultSets=true;Integrated Security=False");
            optionsBuilder.UseSqlServer("Server=.;Database=IranJobDB;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Job>()
                .Property(x => x.CompanyId)
                .IsRequired();
            builder.Entity<Job>()
                .Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(500);
            builder.Entity<Job>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);
            // relations
            builder.Entity<Job>()
                .HasOne(x => x.Province)
                .WithMany(x => x.Jobs)
                .HasForeignKey(x => x.ProvinceId);
            builder.Entity<Job>()
                .HasOne(x => x.Company)
                .WithMany(x => x.Jobs)
                .HasForeignKey(x => x.CompanyId);
            builder.Entity<Job>()
                .HasOne(x => x.JobCategory)
                .WithMany(x => x.Jobs)
                .HasForeignKey(x => x.JobCategoryId);
            builder.Entity<JobRequest>()
                .HasOne(x => x.User)
                .WithMany(x => x.JobRequests)
                .HasForeignKey(x => x.UserId);
            builder.Entity<JobRequest>()
                .HasOne(x => x.Job)
                .WithMany(x => x.JobRequests)
                .HasForeignKey(x => x.JobId);
            builder.Entity<AppUser>()
                .Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(150);
            base.OnModelCreating(builder);
        }
    }
}