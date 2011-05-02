using System.Collections.Generic;
using NES.Sample.Dtos;

namespace NES.Sample.Services
{
    public interface IMessagesService
    {
        IEnumerable<MessageDto> Get();
    }
}