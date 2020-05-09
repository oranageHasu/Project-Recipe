using CoreService.Models.AWS_Cognito_System;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace RecipeService.Models
{
    public class User : CognitoLogin
    {
        [Key]
        [JsonProperty("userId")]
        public Guid? UserId { get; set; }
    }
}
