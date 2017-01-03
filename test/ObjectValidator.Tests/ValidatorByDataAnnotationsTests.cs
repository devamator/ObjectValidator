using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ObjectValidator.Tests
{
   [TestClass]
   public class ValidatorByDataAnnotationsTests
   {
      public ValidatorByDataAnnotationsTests()
      {
         Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
         Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
      }

      [TestMethod]
      public void Validate_ObjectWithoutAnnotations_ReturnEmptyResult()
      {
         var item = new object();
         var result = new ValidatorByDataAnnotations().Validate(item);
         Assert.IsFalse(result.Any(), "Validation shouldn't return any validation result");
      }

      [TestMethod]
      public void Validate_ObjectWithAnnotations_ReturnValidationError()
      {
         object item = new FakeObjectWithAnnotations();
         var result = new ValidatorByDataAnnotations().Validate(item).ToArray();
         Assert.IsTrue(result.Any(), "Validation should return validation result");
         Assert.AreEqual("FakeRequireMessage", result.Single().ErrorMessage, "Validation result should have expected message");
         Assert.AreEqual(nameof(FakeObjectWithAnnotations.RequiredField), result.Single().MemberNames.Single(), "Validation result should have expected member");
      }

      [TestMethod]
      public void Validate_ObjectWithDisplayAttribute_ReturnValidationErrorWithDisplayName()
      {
         object item = new FakeObjectWithDisplayAttribute();
         var result = new ValidatorByDataAnnotations().Validate(item).ToArray();
         Assert.IsTrue(result.Any(), "Validation should return validation result");
         Assert.AreEqual("The RequiredFieldDisplayName field is required.", result.Single().ErrorMessage, "Validation result should contains property display name");
         Assert.AreEqual("RequiredField", result.Single().MemberNames.Single(), "Validation result should have expected member");
      }

      [TestMethod]
      public void Validate_ObjectWithHierarchy_ReturnPropertyValidationError()
      {
         object item = new FakeObjectWithHierarchy();
         var result = new ValidatorByDataAnnotations().Validate(item).ToArray();
         Assert.IsTrue(result.Any(), "Validation should return validation result");
         Assert.AreEqual("The ObjectField.RequiredFieldDisplayName field is required.", result.Single().ErrorMessage, "Validation result should have expected message for property");
         Assert.AreEqual("ObjectField.RequiredField", result.Single().MemberNames.Single(), "Validation result should have expected member");
      }

      [TestMethod]
      public void Validate_ObjectWithHierarchyWithDisplay_ReturnPropertyValidationError()
      {
         object item = new FakeObjectWithHierarchyWithDisplay();
         var result = new ValidatorByDataAnnotations().Validate(item).ToArray();
         Assert.IsTrue(result.Any(), "Validation should return validation result");
         Assert.AreEqual("The RequiredObjectField.RequiredFieldDisplayName field is required.", result.Single().ErrorMessage, "Validation result should have expected message for property");
         Assert.AreEqual("ObjectField.RequiredField", result.Single().MemberNames.Single(), "Validation result should have expected member");
      }

      internal class FakeObjectWithAnnotations
      {
         [Required(ErrorMessage = "FakeRequireMessage")]
         public object RequiredField { get; set; }
      }

      internal class FakeObjectWithDisplayAttribute
      {
         [Required]
         [Display(Name = "RequiredFieldDisplayName")]
         public object RequiredField { get; set; }
      }

      internal class FakeObjectWithHierarchy
      {
         public FakeObjectWithDisplayAttribute ObjectField { get; set; } = new FakeObjectWithDisplayAttribute();
      }

      internal class FakeObjectWithHierarchyWithDisplay
      {
         [Display(Name = "RequiredObjectField")]
         public FakeObjectWithDisplayAttribute ObjectField { get; set; } = new FakeObjectWithDisplayAttribute();
      }
   }
}