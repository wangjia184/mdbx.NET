using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    public interface ISerializer<T>
    {
        byte[] Serialize(T t);
        T Deserialize(byte[] buffer);
    }
}
