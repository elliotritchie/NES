using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NES.Sample.Dtos;

namespace NES.Sample.Data
{
    public class DataRepository : IDataRepository
    {
        private static readonly string _userDtosPath = Path.GetTempPath() + @"\UserDtos.xml";
        private static readonly string _messageDtosPath = Path.GetTempPath() + @"\MessageDtos.xml";

        static DataRepository()
        {
            new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("UserDtos")).Save(_userDtosPath);
            new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("MessageDtos")).Save(_messageDtosPath);
        }

        public IEnumerable<UserDto> UserDtos
        {
            get
            {
                return XDocument.Load(_userDtosPath).Root
                    .Elements("UserDto")
                    .Select(u => new UserDto
                                     {
                                         UserId = (Guid)u.Element("UserId"),
                                         Username = (string)u.Element("Username")
                                     })
                    .ToList();
            }
        }

        public IEnumerable<MessageDto> MessageDtos
        {
            get
            {
                return XDocument.Load(_messageDtosPath).Root
                    .Elements("MessageDto")
                    .Select(u => new MessageDto
                                     {
                                         Username = (string)u.Element("Username"),
                                         Message = (string)u.Element("Message"),
                                         Sent = (DateTime)u.Element("Sent")
                                     })
                    .ToList();
            }
        }

        public void Add(UserDto userDto)
        {
            var doc = XDocument.Load(_userDtosPath);

            doc.Root.Add(new XElement("UserDto",
                                      new XElement("UserId", userDto.UserId),
                                      new XElement("Username", userDto.Username)));

            doc.Save(_userDtosPath);
        }

        public void Add(MessageDto messageDto)
        {
            var doc = XDocument.Load(_messageDtosPath);

            doc.Root.Add(new XElement("MessageDto",
                                      new XElement("Username", messageDto.Username),
                                      new XElement("Message", messageDto.Message),
                                      new XElement("Sent", messageDto.Sent)));

            doc.Save(_messageDtosPath);
        }
    }
}