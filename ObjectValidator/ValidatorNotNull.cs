using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectValidator
{
    public class ValidatorNotNull : IValidator
    {
        public IEnumerable<ValidationResult> Validate(object value)
        {
            if (value == null)
                yield return new ValidationResult("Required value is null.");
        }
    }
}
