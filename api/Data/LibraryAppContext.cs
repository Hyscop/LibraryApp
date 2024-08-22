using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class LibraryAppContext : DbContext
    {

        public LibraryAppContext(DbContextOptions<LibraryAppContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserStats> UserStats { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserBookProgress> UserBookProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring one-to-one relationship between User and UserStats
            modelBuilder.Entity<UserStats>()
                .HasKey(us => us.UserStatsId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserStats)
                .WithOne(us => us.User)
                .HasForeignKey<UserStats>(us => us.UserId);

            // Configuring one-to-many relationship between User and UserBookProgress
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserBookProgresses)
                .WithOne(ubp => ubp.User)
                .HasForeignKey(ubp => ubp.UserId);

            // Configuring one-to-many relationship between Book and UserBookProgress
            modelBuilder.Entity<Book>()
                .HasMany(b => b.UserBookProgresses)
                .WithOne(ubp => ubp.Book)
                .HasForeignKey(ubp => ubp.BookId);

            // Finalizing the configuration
            base.OnModelCreating(modelBuilder);
        }


    }
}