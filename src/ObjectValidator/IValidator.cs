using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectValidator
{
   /// <summary>
   /// Validate object
   /// </summary>
   public interface IValidator
   {
      /// <summary>
      /// Performs validation <paramref name="value" />
      /// </summary>
      /// <param name="value">Object to validate</param>
      /// <returns>Validation results</returns>
      IEnumerable<ValidationResult> Validate(object value);
   }
}
