using System;
using System.Threading;

namespace NES.Sample.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public Guid UserId
        {
            get
            {
                return new Guid(Thread.CurrentPrincipal.Identity.Name);
            }
        }
    }
}