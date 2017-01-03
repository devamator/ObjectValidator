using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObjectValidator
{
   /// <summary>
   /// Implamants validator for any objects implementet <see cref="IValidatableObject"/>
   /// </summary>
   public class ValidatorByInterface : IValidator
   {
      /// <summary>
      /// Performs validation <paramref name="value" />
      /// </summary>
      /// <param name="value">Object to validate</param>
      /// <returns>Validation results</returns>
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
