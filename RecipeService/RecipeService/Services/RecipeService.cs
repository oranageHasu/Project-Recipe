using CoreService.Classes.Error_Logging_System;
using Microsoft.EntityFrameworkCore;
using RecipeService.Classes;
using RecipeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeService.Services
{
    public class RecipeService
    {
        #region Constants 

        private const string ERROR_INSERT_RECIPE = "RecipeService.Post() - ERROR - Failed saving the Recipe, as it was invalid.";
        private const string ERROR_DELETE_RECIPE = "RecipeService.Delete() - ERROR - Failed deleting the Recipe, as it was invalid.";

        #endregion
        #region Private Variables

        private readonly DatabaseContext _context;
        private readonly ErrorLogger _logger;

        #endregion
        #region Constructors

        public RecipeService(DatabaseContext context, ErrorLogger logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion
        #region Public Methods

        public List<Recipe> GetRecipes(RecipeFilter filter)
        {
            List<Recipe> retval = null;
            IQueryable<Recipe> query;

            try
            {
                // Generate the Recipe Query
                query = RecipeQuery(filter);

                // Execute Query
                retval = query.ToList();

                // Post Query Manipulation
                foreach (Recipe recipe in retval)
                {
                    // Sort the Instructions
                    recipe.Instructions = recipe.Instructions.OrderBy(i => i.SortOrder).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        public bool Post(ref Recipe recipe)
        {
            bool retval = false;

            try
            {
                if (recipe != null)
                {
                    _context.Recipes.Add(recipe);
                    _context.SaveChanges();

                    retval = true;
                }
                else
                    throw new Exception(ERROR_INSERT_RECIPE);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        public bool Put(Recipe recipe)
        {
            bool retval = false;

            try
            {
                _context.Recipes.Update(recipe);
                _context.SaveChanges();

                retval = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        public bool Delete(Guid? recipeId)
        {
            bool retval = false;
            Recipe recipe = null;

            try
            {
                // Get the Recipe matching the supplied ID
                recipe = _context.Recipes
                    .Where(r => r.RecipeId == recipeId)
                    .FirstOrDefault();

                if (recipe != null)
                {
                    // Set the IsDeleted flag, indicidating this Recipe was deleted
                    recipe.IsDeleted = true;

                    // Update the database/save
                    _context.Recipes.Update(recipe);
                    _context.SaveChanges();

                    // Indicate to the caller that we succeeded
                    retval = true;
                }
                else
                    throw new Exception(ERROR_DELETE_RECIPE);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        #endregion
        #region Private Methods

        private IQueryable<Recipe> RecipeQuery(RecipeFilter filter)
        {
            IQueryable<Recipe> query = null;

            try
            {
                if (filter != null)
                {
                    // Core Query
                    query = _context.Recipes.Select(r => r);

                    // Include Instructions
                    if (filter.IncludeInstructions)
                        query = query.Include(r => r.Instructions);

                    // Include Ingredients
                    if (filter.IncludeIngredients)
                        query = query.Include(r => r.Ingredients);

                    // Where Clauses
                    if (filter.RecipeId != null)
                        query = query.Where(r => r.RecipeId == filter.RecipeId);

                    if (!filter.IncludeDeleted)
                        query = query.Where(f => f.IsDeleted == false);

                    // Order By the Recipe Name ASC
                    query = query.OrderBy(r => r.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return query;
        }

        #endregion
    }
}
