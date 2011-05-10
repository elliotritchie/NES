using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NES.Sample.Services
{
    public class ValidationService : IValidationService
    {
        public void Validate<T>(T obj)
        {
            if (!TypeDescriptor.GetProperties(obj).Cast<PropertyDescriptor>().All(
                p => p.Attributes.OfType<ValidationAttribute>().All(a => a.IsValid(p.GetValue(obj)))))
            {
                throw new ValidationException(string.Format("Validation of {0} failed.", obj.GetType().Name));
            }
        }
    }
}