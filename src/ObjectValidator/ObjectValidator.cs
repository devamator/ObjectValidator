using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObjectValidator
{
   /// <summary>
   /// Implamants validator for any objects
   /// </summary>
   public class ObjectValidator : IValidator
   {
      private IEnumerable<IValidator> _subvalidators;

      /// <summary>
      /// Initializes a new instance of the <see cref="ObjectValidator"/> class.
      /// </summary>
      /// <param name="validators">Validators to be used for validation</param>
      public ObjectValidator(params IValidator[] validators)
      {
         _subvalidators = validators;
      }
      /// <summary>
      /// Initializes a new instance of the <see cref="ObjectValidator"/> class.
      /// Using <see cref="ValidatorNotNull"/>, <see cref="ValidatorByDataAnnotations"/>, <see cref="ValidatorByInterface"/> for validation.
      /// </summary>
      public ObjectValidator()
      {
         _subvalidators = new IValidator[]
         {
                new ValidatorNotNull(),
                new ValidatorByDataAnnotations(),
                new ValidatorByInterface()
         };
      }

      /// <summary>
      /// Performs validation <paramref name="value" />
      /// </summary>
      /// <param name="value">Object to validate</param>
      /// <returns>Validation results</returns>
      public IEnumerable<ValidationResult> Validate(object value)
      {
         return _subvalidators
             .SelectMany(v => v.Validate(value))
             .Where(v => v != ValidationResult.Success);
      }
   }
}
