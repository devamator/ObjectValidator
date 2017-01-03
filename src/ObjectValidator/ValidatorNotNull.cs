using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectValidator
{
   /// <summary>
   /// Implamants validator for required object (is not null)
   /// </summary>
   public class ValidatorNotNull : IValidator
   {
      /// <summary>
      /// Performs validation <paramref name="value" />
      /// </summary>
      /// <param name="value">Object to validate</param>
      /// <returns>Validation results</returns>
      public IEnumerable<ValidationResult> Validate(object value)
      {
         if (value == null)
            yield return new ValidationResult("Required value is null.");
      }
   }
}
