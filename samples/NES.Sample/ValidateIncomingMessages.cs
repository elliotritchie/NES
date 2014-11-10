using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NES.Sample.Services;
using NServiceBus.MessageMutator;

namespace NES.Sample
{
    public class ValidateIncomingMessages : IMutateIncomingMessages
    {
        private readonly IValidationService _validationService;

        public ValidateIncomingMessages(IValidationService validationService)
        {
            _validationService = validationService;
        }

        public object MutateIncoming(object message)
        {
            _validationService.Validate(message);
            return message;
        }
    }
}
