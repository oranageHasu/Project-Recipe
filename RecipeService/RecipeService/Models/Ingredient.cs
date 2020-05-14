using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeService.Models
{
    public class Ingredient
    {
        [Key]
        [JsonProperty("ingredientId")]
        public Guid? IngredientId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("recipe")]
        public Recipe Recipe { get; set; }

        [JsonProperty("recipeId")]
        public Guid? RecipeId { get; set; }
    }
}
