using System;
using System.Diagnostics;
using System.Threading;
using System.Web.Security;

namespace NES.Sample.Services
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(Guid userId, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userId.ToString(), createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}