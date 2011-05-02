using System;

namespace NES.Sample.Services
{
    public interface IAuthenticationService
    {
        Guid UserId { get; }
    }
}