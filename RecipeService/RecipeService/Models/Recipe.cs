using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeService.Models
{
    public class Recipe
    {
        [Key]
        [JsonProperty("recipeId")]
        public Guid? RecipeId { get; set; }

        [StringLength(50)]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rating")]
        public decimal Rating { get; set; } = 5.0m;

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public ICollection<Instruction> Instructions { get; set; }
    }
}
