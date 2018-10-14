using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    internal static class Dbi
    {

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int OpenDelegate(IntPtr txn
            , [MarshalAs(UnmanagedType.LPStr)] string name
            , [MarshalAs(UnmanagedType.U4)] int flags
            , out uint dbi);

        private static OpenDelegate _openDelegate = null;

        internal static uint Open(IntPtr txn, string name, DatabaseOption options)
        {
            uint dbi;
            int err = _openDelegate(txn, name, (int)options, out dbi);
            if (err != 0)
                throw new MdbxException("mdbx_dbi_open", err);
            return dbi;
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CloseDelegate(IntPtr env, uint dbi);

        private static CloseDelegate _closeDelegate = null;

        internal static void Close(IntPtr env, uint dbi)
        {
            int err = _closeDelegate(env, dbi);
            if (err != 0)
                throw new MdbxException("mdbx_dbi_close", err);
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int PutDelegate(IntPtr txn
            , uint dbi
            , ref DbValue key
            , ref DbValue value
            , [MarshalAs(UnmanagedType.U4)] uint flags);

        private static PutDelegate _putDelegate = null;

        internal static void Put(IntPtr txn, uint dbi, DbValue key, DbValue value, PutOption options)
        {
            int err = _putDelegate(txn, dbi, ref key, ref value, (uint)options);
            if (err != 0)
                throw new MdbxException("mdbx_put", err);
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int DelDelegate(IntPtr txn
            , uint dbi
            , ref DbValue key
            , IntPtr value);

        private static DelDelegate _delDelegate = null;

        internal static void Del(IntPtr txn, uint dbi, DbValue key, IntPtr value)
        {
            int err = _delDelegate(txn, dbi, ref key, value);
            if (err != 0)
                throw new MdbxException("mdbx_del", err);
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetDelegate(IntPtr txn
            , uint dbi
            , ref DbValue key
            , ref DbValue value);

        private static GetDelegate _getDelegate = null;

        internal static DbValue Get(IntPtr txn, uint dbi, DbValue key)
        {
            DbValue value = new DbValue();
            int err = _getDelegate(txn, dbi, ref key, ref value);
            if (err != 0)
                throw new MdbxException("mdbx_get", err);
            return value;
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int DropDelegate(IntPtr txn, uint dbi, int del);

        private static DropDelegate _dropDelegate = null;

        internal static void Drop(IntPtr txn, uint dbi, bool del)
        {
            int err = _dropDelegate(txn, dbi, del ? 1 : 0);
            if (err != 0)
                throw new MdbxException("mdbx_drop", err);
        }

        internal static void Bind()
        {
            _openDelegate = Library.GetProcAddress<OpenDelegate>("mdbx_dbi_open") as OpenDelegate;
            _closeDelegate = Library.GetProcAddress<CloseDelegate>("mdbx_dbi_close") as CloseDelegate;
            _putDelegate = Library.GetProcAddress<PutDelegate>("mdbx_put") as PutDelegate;
            _getDelegate = Library.GetProcAddress<GetDelegate>("mdbx_get") as GetDelegate;
            _delDelegate = Library.GetProcAddress<DelDelegate>("mdbx_del") as DelDelegate;
            _dropDelegate = Library.GetProcAddress<DropDelegate>("mdbx_drop") as DropDelegate;
        }
    }
}
