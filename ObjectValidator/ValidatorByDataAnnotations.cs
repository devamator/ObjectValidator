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
         return Validate(value, null);
      }

      public IEnumerable<ValidationResult> Validate(object value, ValidationContext context)
      {
         if (value == null)
            return Enumerable.Empty<ValidationResult>();

         var valueType = value.GetType();

         return ValidateProperties(valueType, value, context).Union(ValidateEnumerable(valueType, value, context));
      }

      private IEnumerable<ValidationResult> ValidateProperties(Type type, object @object, ValidationContext context)
      {
         return TypeDescriptor.GetProperties(type)
             .OfType<PropertyDescriptor>()
             .Where(p => !p.IsReadOnly)
             .SelectMany(property => ValidateProperty(property, @object, context));
      }

      private IEnumerable<ValidationResult> ValidateProperty(PropertyDescriptor property, object @object, ValidationContext context)
      {
         var propertyContext = context != null
            ? new ValidationContext(@object, null, null)
            {
               DisplayName = string.Join(".", context.DisplayName, GetDisplayName(property)),
               MemberName = string.Join(".", context.MemberName, property.Name),
            }
            : new ValidationContext(@object, null, null)
            {
               DisplayName = GetDisplayName(property),
               MemberName = property.Name,
            };

         var value = property.GetValue(@object);

         return property.Attributes
             .OfType<ValidationAttribute>()
             .Select(attribute => attribute.GetValidationResult(value, propertyContext))
             .Where(result => result != ValidationResult.Success)
             .Union(Validate(value, propertyContext));
      }

      private IEnumerable<ValidationResult> ValidateEnumerable(Type type, object @object, ValidationContext context)
      {
         var enumerable = @object as IEnumerable;
         if (enumerable == null)
            return Enumerable.Empty<ValidationResult>();

         return enumerable.OfType<object>().SelectMany(item => Validate(item, context));
      }

      private string GetDisplayName(PropertyDescriptor property)
      {
         var displayAttribute = property.Attributes.OfType<DisplayAttribute>().ToArray().FirstOrDefault();
         return displayAttribute != null ? displayAttribute.GetName() : property.DisplayName;
      }
   }
}
