using System.Collections.Generic;
using NES.Sample.Dtos;

namespace NES.Sample.Data
{
    public interface IDataRepository
    {
        IEnumerable<UserDto> UserDtos { get; }
        IEnumerable<MessageDto> MessageDtos { get; }
        void Add(UserDto userDto);
        void Add(MessageDto messageDto);
    }
}