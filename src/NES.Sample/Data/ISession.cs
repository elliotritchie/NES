using System.Collections.Generic;
using NES.Sample.Dtos;

namespace NES.Sample.Data
{
    public interface ISession
    {
        IEnumerable<UserDto> UserDtos { get; }
        IEnumerable<MessageDto> MessageDtos { get; }
        void Add(UserDto userDto);
        void Add(MessageDto messageDto);
    }
}