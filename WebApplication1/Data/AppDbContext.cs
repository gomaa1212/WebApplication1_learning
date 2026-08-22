using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<StudentImages> StudentImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Student>().HasKey(s => s.Id);
            modelBuilder.Entity<Student>().Property(s => s.Name).HasColumnType("nvarchar(100)").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Student>().Property(s => s.FileUrl).HasMaxLength(200);

            modelBuilder.Entity<Student>()
                .HasOne(s=>s.Gender)
                .WithMany(t=>t.Students)
                .HasForeignKey(s=>s.GenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentImages>().ToTable("StudentImages");
            modelBuilder.Entity<StudentImages>()
                .HasOne(s => s.Student)
                .WithMany(t => t.StudentImages)
                .HasForeignKey(s => s.StudentId);

        }
    }
}
