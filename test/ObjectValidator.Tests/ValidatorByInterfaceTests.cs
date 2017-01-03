using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;

namespace ObjectValidator.Tests
{
   [TestClass]
   public class ValidatorByInterfaceTests
   {
      [TestMethod]
      public void Validate_ObjectWithoutValidatableInterface_ReturnEmptyResult()
      {
         var item = new object();
         var result = new ValidatorByInterface().Validate(item);
         Assert.IsFalse(result.Any(), "Validation shouldn't return any validation result");
      }

      [TestMethod]
      public void Validate_ObjectWithValidatableInterface_InvokeValidateMethod()
      {
         var item = new FakeValidatableObject();
         var result = new ValidatorByInterface().Validate(item).ToArray();
         Assert.IsTrue(item.ValidateIsInvoked, "Validation should invoke validation method");
      }

      [TestMethod]
      public void Validate_ObjectWithValidatableInterface_ReturnInterfaceMethodResult()
      {
         var validationResult = new ValidationResult("FakeValidatorMessage");
         var item = new FakeValidatableObject { Result = new[] { validationResult } };
         var result = new ValidatorByInterface().Validate(item).ToArray();
         Assert.IsTrue(result.Any(), "Validation should return validator result");
         Assert.AreSame(validationResult, result.Single(), "Validation result should equal expected");
      }

      internal class FakeValidatableObject : IValidatableObject
      {
         internal IEnumerable<ValidationResult> Result { get; set; } = Enumerable.Empty<ValidationResult>();
         internal bool ValidateIsInvoked { get; private set; }

         public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
         {
            ValidateIsInvoked = true;
            return Result;
         }
      }
   }
}