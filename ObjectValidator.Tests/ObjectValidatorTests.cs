using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjectValidator.Tests
{
   [TestClass]
   public class ObjectValidatorTests
   {
      [TestMethod]
      public void Validate_WithValidators_InvokeAllValidators()
      {
         var validators = new[] {
            new FakeValidator(),
            new FakeValidator(),
            new FakeValidator()
         };
         var item = new object();
         var result = new ObjectValidator(validators).Validate(item).ToArray();
         Assert.IsTrue(validators.All(v => v.ValidateIsInvoked), "Validation should invoke validate method for each validator");
      }

      [TestMethod]
      public void Validate_WithoutValidators_ReturnEmptyResult()
      {
         var item = new object();
         var result = new ObjectValidator(new IValidator[0]).Validate(item);
         Assert.IsFalse(result.Any(), "Validation shouldn't return any validation result");
      }

      [TestMethod]
      public void Validate_WithValidators_ReturnValidatorsErros()
      {
         var validationResult = new ValidationResult("FakeValidatorMessage");
         var validator = new FakeValidator { Result = new[] { validationResult } };
         var item = new object();
         var result = new ObjectValidator(validator).Validate(item).ToArray();
         Assert.IsTrue(result.Any(), "Validation should return validator result");
         Assert.AreSame(validationResult, result.Single(), "Validation result should equal expected");
      }

      [TestMethod]
      public void Validate_WithValidators_IgnoreValidateSuccess()
      {
         var validator = new FakeValidator { Result = new[] { ValidationResult.Success } };
         var item = new object();
         var result = new ObjectValidator(validator).Validate(item);
         Assert.IsFalse(result.Any(), "Validation shouldn't return validation success result");
      }

      internal class FakeValidator : IValidator
      {
         internal IEnumerable<ValidationResult> Result { get; set; } = Enumerable.Empty<ValidationResult>();
         internal bool ValidateIsInvoked { get; private set; }

         public IEnumerable<ValidationResult> Validate(object value)
         {
            ValidateIsInvoked = true;
            return Result;
         }
      }
   }
}
