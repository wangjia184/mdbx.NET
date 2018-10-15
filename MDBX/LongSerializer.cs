using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    public class LongSerializer : ISerializer<long>
    {
        public long Deserialize(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return 0;

            return BitConverter.ToInt64(buffer, 0);
        }

        public byte[] Serialize(long num)
        {
            return BitConverter.GetBytes(num);
        }
    }
}
