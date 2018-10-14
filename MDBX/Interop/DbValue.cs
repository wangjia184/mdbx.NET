using System;
using System.Runtime.InteropServices;

namespace MDBX.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DbValue
    {
        internal IntPtr Address { get; }
        private readonly IntPtr _size;
        
        internal int Length { get { return _size.ToInt32(); } }

        internal DbValue(IntPtr addr, int length)
        {
            this.Address = addr;
            this._size = IntPtr.Add(IntPtr.Zero, length);
        }
    }
}
