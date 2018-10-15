using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    internal static class Cursor
    {


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int OpenDelegate(IntPtr txn, uint dbi, out IntPtr cursor);

        private static OpenDelegate _openDelegate = null;

        internal static IntPtr Open(IntPtr txn, uint dbi)
        {
            IntPtr ptr;
            int err = _openDelegate(txn, dbi, out ptr);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_open", err);
            return ptr;
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CloseDelegate(IntPtr cursor);

        private static CloseDelegate _closeDelegate = null;

        internal static void Close(IntPtr cursor)
        {
            _closeDelegate(cursor);
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetDelegate(IntPtr cursor, ref DbValue key, ref DbValue value, CursorOp op);

        private static GetDelegate _getDelegate = null;

        internal static void Get(IntPtr cursor, ref DbValue key, ref DbValue value, CursorOp op)
        {
            int err = _getDelegate(cursor,ref key,ref value, op);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_get", err);
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int PutDelegate(IntPtr cursor, ref DbValue key, ref DbValue value, CursorPutOption option);

        private static PutDelegate _putDelegate = null;

        internal static void Put(IntPtr cursor, ref DbValue key, ref DbValue value, CursorPutOption option)
        {
            int err = _putDelegate(cursor, ref key, ref value, option);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_put", err);
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int DelDelegate(IntPtr cursor, CursorDelOption option);

        private static DelDelegate _delDelegate = null;

        internal static void Del(IntPtr cursor, CursorDelOption option)
        {
            int err = _delDelegate(cursor, option);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_del", err);
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CountDelegate(IntPtr cursor, ref IntPtr count);

        private static CountDelegate _countDelegate = null;

        internal static int Count(IntPtr cursor)
        {
            IntPtr count = IntPtr.Zero;
            int err = _countDelegate(cursor, ref count);
            if (err != 0)
                throw new MdbxException("mdbx_cursor_count", err);
            return count.ToInt32();
        }

        internal static void Bind()
        {
            _closeDelegate = Library.GetProcAddress<CloseDelegate>("mdbx_cursor_close") as CloseDelegate;
            _openDelegate = Library.GetProcAddress<OpenDelegate>("mdbx_cursor_open") as OpenDelegate;
            _getDelegate = Library.GetProcAddress<GetDelegate>("mdbx_cursor_get") as GetDelegate;
            _putDelegate = Library.GetProcAddress<PutDelegate>("mdbx_cursor_put") as PutDelegate;
            _delDelegate = Library.GetProcAddress<DelDelegate>("mdbx_cursor_del") as DelDelegate;
            _countDelegate = Library.GetProcAddress<CountDelegate>("mdbx_cursor_count") as CountDelegate;
        }
    }
}
