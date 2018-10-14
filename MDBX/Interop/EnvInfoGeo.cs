using System;
using System.Runtime.InteropServices;



namespace MDBX.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EnvInfoGeo
    {
        [MarshalAs(UnmanagedType.U8)]
        private ulong _lower;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _upper;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _current;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _shrink;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _grow;

        /// <summary>
        /// lower limit for datafile size
        /// </summary>
        public ulong LowerLimit { get { return _lower; } }

        /// <summary>
        /// upper limit for datafile size
        /// </summary>
        public ulong UpperLimit { get { return _upper; } }

        /// <summary>
        /// current datafile size
        /// </summary>
        public ulong CurrentSize { get { return _current; } }

        /// <summary>
        /// shrink threshold for datafile
        /// </summary>
        public ulong ShrinkThreshold { get { return _shrink; } }

        /// <summary>
        /// growth step for datafile
        /// </summary>
        public ulong GrowStep { get { return _grow; } }
    }

}
