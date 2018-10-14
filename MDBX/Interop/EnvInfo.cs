using System;
using System.Runtime.InteropServices;



namespace MDBX.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EnvInfo
    {
        private EnvInfoGeo _geo;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _mapSize;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _lastPageNo;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _recentTxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _lastReaderTxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _selfLastReaderTxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta0TxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta0TxnSign;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta1TxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta1TxnSign;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta2TxnID;

        [MarshalAs(UnmanagedType.U8)]
        private ulong _meta2TxnSign;

        [MarshalAs(UnmanagedType.U4)]
        private uint _maxReaders;

        [MarshalAs(UnmanagedType.U4)]
        private uint _numOfReaders;

        [MarshalAs(UnmanagedType.U4)]
        private uint _dxbPageSize;

        [MarshalAs(UnmanagedType.U4)]
        private uint _sysPageSize;


    }

}
