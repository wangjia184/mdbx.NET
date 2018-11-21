using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    public class ByteArraySerializer : ISerializer<byte[]>
    {
        public byte[] Deserialize(byte[] buffer)
        {
            return buffer;
        }

        public byte[] Serialize(byte[] buffer)
        {
            return buffer;
        }
    }
}
