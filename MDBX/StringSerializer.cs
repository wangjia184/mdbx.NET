using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    public class StringSerializer : ISerializer<string>
    {
        public string Deserialize(byte[] buffer)
        {
            if (buffer == null)
                return null;

            return Encoding.UTF8.GetString(buffer);
        }

        public byte[] Serialize(string text)
        {
            if (text == null)
                return new byte[0];

            return Encoding.UTF8.GetBytes(text);
        }
    }
}
