using System.ComponentModel.DataAnnotations;
using TextMatch.Domain.Constants;
using TextMatch.Model.Validators;

namespace TextMatch.Model.Request
{
    public class TextMatchingRequest
    {
        [Required(ErrorMessage = ErrorMessages.MainTextIsRequiredErrorMsg)]
        public string MainText { get; set; } = string.Empty;

        [Required(ErrorMessage = ErrorMessages.SubTextIsRequiredErrorMsg)]
        [StringSmallerThan("MainText", ErrorMessage = ErrorMessages.SubTextMustBeShorterThanMainTextErrorMsg)]
        public string SubText { get; set; } = string.Empty;
    }
}
