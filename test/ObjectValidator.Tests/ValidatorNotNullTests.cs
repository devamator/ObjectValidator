using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ObjectValidator.Tests
{
   [TestClass]
   public class ValidatorNotNullTests
   {
      [TestMethod]
      public void Validate_NotNullObject_ReturnEmptyResult()
      {
         var item = new object();
         var result = new ValidatorNotNull().Validate(item);
         Assert.IsFalse(result.Any(), "Validation shouldn't return any validation result");
      }

      [TestMethod]
      public void Validate_NullObject_ReturnValidationError()
      {
         object item = null;
         var result = new ValidatorNotNull().Validate(item).ToArray();
         Assert.IsTrue(result.Any(), "Validation should return validation result");
         Assert.AreEqual(1, result.Length, "Validation should return one validation result");
         Assert.AreEqual("Required value is null.", result[0].ErrorMessage, "Validation result should have expected message");
         Assert.IsFalse(result[0].MemberNames.Any(), "Validation result should have empty members collection");
      }
   }
}