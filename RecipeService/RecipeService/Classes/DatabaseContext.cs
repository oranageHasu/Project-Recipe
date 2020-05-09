using Microsoft.EntityFrameworkCore;
using RecipeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeService.Classes
{
    public class DatabaseContext : DbContext
    {
        #region Public Variables

        public DbSet<User> Users { get; set; }

        #endregion
        #region Constructors

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        #endregion
        #region Public Method Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                SetupDatabaseSeedData(ref modelBuilder);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion
        #region Database Seeded Data

        // Driver Method for sseeding a new database
        private void SetupDatabaseSeedData(ref ModelBuilder modelBuilder)
        {
            Guid? userId;

            // Seed data for the database here
            userId = SeedUser(ref modelBuilder);
        }

        #region Users

        private Guid? SeedUser(ref ModelBuilder modelBuilder)
        {
            Guid? retval = null;

            try
            {
                retval = Guid.NewGuid();

                modelBuilder.Entity<User>()
                .HasData(new User
                {
                    UserId = retval,
                    UserName = "admin",
                    Password = "admin"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "DatabaseContext - Failed to seed User data");
            }

            return retval;
        }

        #endregion

        #endregion
    }
}
