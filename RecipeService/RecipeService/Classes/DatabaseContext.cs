using Microsoft.EntityFrameworkCore;
using RecipeService.Models;
using System;
using System.Collections.Generic;

namespace RecipeService.Classes
{
    public class DatabaseContext : DbContext
    {
        #region Public Variables

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Instruction> Instructions { get; set; }

        #endregion
        #region Constructors

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        #endregion
        #region Public Method Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                // Seed the database
                SetupDatabaseSeedData(ref modelBuilder);

                // Perform database schema setup
                SetRecipeRelationships(ref modelBuilder);
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
            List<Recipe> recipes;

            // Seed data for the database here
            userId = SeedUser(ref modelBuilder);
            recipes = SeedRecipe(ref modelBuilder);
            SeedInstructions(ref modelBuilder, recipes);
            SeedIngredients(ref modelBuilder, recipes);
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
        #region Recipes

        private List<Recipe> SeedRecipe(ref ModelBuilder modelBuilder)
        {
            List<Recipe> retval = new List<Recipe>();
            Recipe recipe;

            try
            {
                retval.Add(recipe = new Recipe
                {
                    RecipeId = Guid.NewGuid(),
                    Name = "Pho",
                    Rating = 4.7m,
                    Notes = "For best results, use shank and knee bones.",
                    IsDeleted = false
                });
                modelBuilder.Entity<Recipe>().HasData(recipe);

                retval.Add(recipe = new Recipe
                {
                    RecipeId = Guid.NewGuid(),
                    Name = "Polish Perogies",
                    Rating = 4.1m,
                    Notes = "For best results, choose potatoes that have as little water in them as possible such as Russets.",
                    IsDeleted = false
                });
                modelBuilder.Entity<Recipe>().HasData(recipe);

                retval.Add(recipe = new Recipe
                {
                    RecipeId = Guid.NewGuid(),
                    Name = "Bread",
                    Rating = 3.2m,
                    Notes = "Use active yeast for best results.",
                    IsDeleted = false
                });
                modelBuilder.Entity<Recipe>().HasData(recipe);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "DatabaseContext - Failed to seed Recipe data");
            }

            return retval;
        }

        private void SeedInstructions(ref ModelBuilder modelBuilder, List<Recipe> recipes)
        {
            string instruction;

            try
            {
                foreach (Recipe recipe in recipes)
                {
                    for(int i=1; i<=3; i++)
                    {
                        instruction = i switch
                        {
                            1 => "Preheat oven to 450F.",
                            2 => "Insert food.",
                            3 => "Take out food after 15 minutes.",
                            _ => "If this happens, I should quit my day job.",
                        };

                        SeedInstruction(ref modelBuilder, recipe, instruction, i);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "DatabaseContext - Failed to seed Instruction data");
            }
        }

        private void SeedInstruction(ref ModelBuilder modelBuilder, Recipe recipe, string instruction, int sortOrder)
        {
            modelBuilder.Entity<Instruction>()
            .HasData(new Instruction
            {
                InstructionId = Guid.NewGuid(),
                RecipeId = recipe.RecipeId,
                Value = instruction,
                SortOrder = sortOrder
            });
        }


        private void SeedIngredients(ref ModelBuilder modelBuilder, List<Recipe> recipes)
        {
            string ingredient;

            try
            {
                foreach (Recipe recipe in recipes)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        ingredient = i switch
                        {
                            1 => "Water",
                            2 => "Flour",
                            3 => "Salt",
                            _ => "If this happened, I should quit my day job.",
                        };

                        SeedIngredient(ref modelBuilder, recipe, ingredient);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "DatabaseContext - Failed to seed Ingredient data");
            }
        }

        private void SeedIngredient(ref ModelBuilder modelBuilder, Recipe recipe, string ingredient)
        {
            modelBuilder.Entity<Ingredient>()
            .HasData(new Ingredient
            {
                IngredientId = Guid.NewGuid(),
                RecipeId = recipe.RecipeId,
                Value = ingredient
            });
        }

        #endregion

        #endregion
        #region Database Relationships

        private void SetRecipeRelationships(ref ModelBuilder modelBuilder)
        {
            // Recipe/Ingredient Relationship
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe);

            // Recipe/Instruction Relationship
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Instructions)
                .WithOne(i => i.Recipe);
        }

        #endregion
    }
}
