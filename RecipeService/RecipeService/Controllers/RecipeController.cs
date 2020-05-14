using CoreService.Classes.Error_Logging_System;
using Microsoft.AspNetCore.Mvc;
using RecipeService.Models;
using System;
using System.Collections.Generic;
using RecipeService.Services;

namespace RecipeService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RecipeController : ControllerBase
    {
        #region Private Variables

        private readonly Services.RecipeService _service;
        private readonly ErrorLogger _logger;

        #endregion
        #region Constructors

        public RecipeController(
            Services.RecipeService service,
            ErrorLogger logger)
        {
            _service = service;
            _logger = logger;
        }

        #endregion
        #region Public Methods

        [HttpGet]
        public ActionResult Get([FromQuery] RecipeFilter filter)
        {
            ActionResult retval = BadRequest();
            List<Recipe> recipes;

            try
            {
                recipes = _service.GetRecipes(filter);

                if (recipes != null)
                {
                    if (filter.RecipeId != null && recipes.Count > 0)
                        retval = new OkObjectResult(recipes[0]);
                    else
                        retval = new OkObjectResult(recipes);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Recipe recipe)
        {
            ActionResult retval = BadRequest();

            try
            {
                if (!ModelState.IsValid)
                {
                    retval = BadRequest(ModelState);
                }
                else
                {
                    if (_service.Post(ref recipe))
                        retval = new OkObjectResult(recipe);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        [HttpPut]
        public ActionResult Put([FromBody] Recipe recipe)
        {
            ActionResult retval = BadRequest();

            try
            {
                if (!ModelState.IsValid)
                {
                    retval = BadRequest(ModelState);
                }
                else
                {
                    if (_service.Put(recipe))
                        retval = new OkObjectResult(recipe);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        [HttpDelete("{recipeId}")]
        public ActionResult Delete(Guid? recipeId)
        {
            ActionResult retval = BadRequest();

            try
            {
                if (_service.Delete(recipeId))
                    retval = Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }

            return retval;
        }

        #endregion
    }
}
