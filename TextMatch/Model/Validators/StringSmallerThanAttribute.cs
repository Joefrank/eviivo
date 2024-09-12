using System.ComponentModel.DataAnnotations;


namespace TextMatch.Model.Validators
{

    public class StringSmallerThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public StringSmallerThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the current value of the property to validate
            var currentValue = value as string;

            // Get the value of the property to compare with
            var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);
            var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance) as string;

            // If both values are not null, compare lexicographically
            if (currentValue != null && comparisonValue != null)
            {
                if (currentValue.Length > comparisonValue.Length)
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be smaller than {_comparisonProperty}.");
                }
            }

            return ValidationResult.Success;
        }
    }

}