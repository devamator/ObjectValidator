using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObjectValidator
{
   public class ObjectValidator : IValidator
   {
      private IEnumerable<IValidator> _subvalidators;

      public ObjectValidator(params IValidator[] validators)
      {
         _subvalidators = validators;
      }
      public ObjectValidator()
      {
         _subvalidators = new IValidator[]
         {
                new ValidatorNotNull(),
                new ValidatorByDataAnnotations(),
                new ValidatorByInterface()
         };
      }

      public IEnumerable<ValidationResult> Validate(object value)
      {
         return _subvalidators
             .SelectMany(v => v.Validate(value))
             .Where(v => v != ValidationResult.Success);
      }
   }
}
