using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObjectValidator
{
    public class ValidatorByDataAnnotations : IValidator
    {
        public IEnumerable<ValidationResult> Validate(object value)
        {
            if (value == null)
                return Enumerable.Empty<ValidationResult>();

            var valueType = value.GetType();

            return ValidateProperties(valueType, value).Union(ValidateEnumerable(valueType, value));
        }

        private IEnumerable<ValidationResult> ValidateProperties(Type type, object @object)
        {
            return TypeDescriptor.GetProperties(type)
                .OfType<PropertyDescriptor>()
                .Where(p => !p.IsReadOnly)
                .SelectMany(property => ValidateProperty(property, @object));
        }

        private IEnumerable<ValidationResult> ValidateProperty(PropertyDescriptor property, object @object)
        {
            var context = new ValidationContext(@object, null, null)
            {
                DisplayName = property.DisplayName,
                MemberName = property.Name
            };

            var value = property.GetValue(@object);
            
            return property.Attributes
                .OfType<ValidationAttribute>()
                .Select(attribute => attribute.GetValidationResult(value, context))
                .Where(result => result != ValidationResult.Success)
                .Union(Validate(value));
        }

        private IEnumerable<ValidationResult> ValidateEnumerable(Type type, object @object)
        {
            var enumerable = @object as IEnumerable;
            if(enumerable == null)
                return Enumerable.Empty<ValidationResult>();

            return enumerable.OfType<object>().SelectMany(Validate);
        }
    }
}
