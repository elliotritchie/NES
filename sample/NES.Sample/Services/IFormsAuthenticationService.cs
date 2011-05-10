using System;

namespace NES.Sample.Services
{
    public interface IFormsAuthenticationService
    {
        void SignIn(Guid userId, bool createPersistentCookie);
        void SignOut();
    }
}