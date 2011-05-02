namespace NES.Sample.Services
{
    public interface IValidationService
    {
        void Validate<T>(T obj);
    }
}