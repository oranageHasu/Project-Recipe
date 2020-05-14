using System;

namespace RecipeService.Models
{
    public class RecipeFilter
    {
        public Guid? RecipeId { get; set; } = null;
        public bool IncludeDeleted { get; set; } = false;
        public bool IncludeInstructions { get; set; } = true;
        public bool IncludeIngredients { get; set; } = true;

    }
}
