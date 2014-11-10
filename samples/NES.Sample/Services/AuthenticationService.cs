using System;
using System.Threading;

namespace NES.Sample.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private Guid currentUser;

        public Guid UserId
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
    }
}