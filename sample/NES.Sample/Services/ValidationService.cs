using System.ComponentModel.DataAnnotations;

namespace NES.Sample.Services
{
    public class ValidationService : IValidationService
    {
        public void Validate<T>(T obj)
        {
            Validator.ValidateObject(obj, new ValidationContext(obj, null, null));
        }
    }
}