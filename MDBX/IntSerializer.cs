using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    public class IntSerializer : ISerializer<int>
    {
        public int Deserialize(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                return 0;

            return BitConverter.ToInt32(buffer, 0);
        }

        public byte[] Serialize(int num)
        {
            return BitConverter.GetBytes(num);
        }
    }
}
