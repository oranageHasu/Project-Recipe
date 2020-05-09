using Newtonsoft.Json;
using static CoreService.Classes.Enumerations;

namespace RecipeService.Classes
{
    public class LoggedError
    {
        [JsonProperty("errorType")]
        public ErrorLevel ErrorType { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }
    }
}