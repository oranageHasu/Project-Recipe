using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using static RecipeService.Classes.Constants;

namespace RecipeService.Models
{
    public class Instruction
    {
        [Key]
        [JsonProperty("instructionId")]
        public Guid? InstructionId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("sortOrder")]
        public int SortOrder { get; set; } = Invalid_Sort;

        [JsonProperty("recipe")]
        public Recipe Recipe { get; set; }

        [JsonProperty("recipeId")]
        public Guid? RecipeId { get; set; }
    }
}
