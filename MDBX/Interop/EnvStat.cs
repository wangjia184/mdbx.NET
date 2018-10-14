using System;
using System.Runtime.InteropServices;


namespace MDBX.Interop
{
    /// <summary>
    /// Statistics for a database in the environment
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct EnvStat
    {
        [MarshalAs(UnmanagedType.U4)]
        private uint _pageSize;

        [MarshalAs(UnmanagedType.U4)]
        private uint _depth;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _branchPages;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _leafPages;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _overflowPages;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _entries;

        /// <summary>
        /// Size of a database page. This is currently the same for all databases.
        /// </summary>
        public uint PageSize { get { return _pageSize; } }

        /// <summary>
        /// Depth (height) of the B-tree
        /// </summary>
        public uint Depth { get { return _depth;} }

        /// <summary>
        /// Number of internal (non-leaf) pages
        /// </summary>
        public ulong BranchPages { get { return _branchPages;} }

        /// <summary>
        /// Number of leaf pages
        /// </summary>
        public ulong LeafPages { get { return _leafPages;} }

        /// <summary>
        /// Number of overflow pages
        /// </summary>
        public ulong OverflowPages { get { return _overflowPages;} }

        /// <summary>
        /// Number of data items
        /// </summary>
        public ulong Entries { get { return _entries;} }
    }
}
