using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectValidator
{
    public interface IValidator
    {
        IEnumerable<ValidationResult> Validate(object value);
    }
}
