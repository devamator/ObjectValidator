using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObjectValidator
{
    public class ValidatorByInterface : IValidator
    {
        public IEnumerable<ValidationResult> Validate(object value)
        {
            var validatableValue = value as IValidatableObject;
            if (validatableValue == null)
                return Enumerable.Empty<ValidationResult>();

            var context = new ValidationContext(value, null, null);
            return validatableValue.Validate(context) ?? Enumerable.Empty<ValidationResult>();
        }
    }
}
