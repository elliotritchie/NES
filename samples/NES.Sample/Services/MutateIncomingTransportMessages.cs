using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.MessageMutator;

namespace NES.Sample.Services
{
    public class MutateIncomingTransportMessages : IMutateIncomingTransportMessages
    {
        private readonly AuthenticationService _authenticationService;

        public MutateIncomingTransportMessages(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public void MutateIncoming(TransportMessage transportMessage)
        {
            var headerUserId = transportMessage.Headers[Headers.WindowsIdentityName];
            Guid userId;
            if (Guid.TryParse(headerUserId, out userId))
            {
                this._authenticationService.UserId = userId;
            }
        }
    }
}
