using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    internal static class Txn
    {
        /// <summary>
        /// mdbx_txn_begin
        /// </summary>
        /// <param name="env"></param>
        /// <param name="parent"></param>
        /// <param name="flags"></param>
        /// <param name="txn"></param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int BeginDelegate(IntPtr env, IntPtr parent, [MarshalAs(UnmanagedType.U4)] int flags, out IntPtr txn);

        private static BeginDelegate _beginDelegate = null;

        internal static IntPtr Begin(IntPtr env, IntPtr parent, TransactionOption flags)
        {
            IntPtr ptr;
            int err = _beginDelegate(env, parent, (int)flags, out ptr);
            if (err != 0)
                throw new MdbxException("mdbx_txn_begin", err);
            return ptr;
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CommitDelegate(IntPtr txn);

        private static CommitDelegate _commitDelegate = null;

        internal static void Commit(IntPtr txn)
        {
            int err = _commitDelegate(txn);
            if (err != 0)
                throw new MdbxException("mdbx_txn_commit", err);
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int AbortDelegate(IntPtr txn);

        private static AbortDelegate _abortDelegate = null;

        internal static void Abort(IntPtr txn)
        {
            int err = _abortDelegate(txn);
            if (err != 0)
                throw new MdbxException("mdbx_txn_abort", err);
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ResetDelegate(IntPtr txn);

        private static ResetDelegate _resetDelegate = null;

        internal static void Reset(IntPtr txn)
        {
            int err = _resetDelegate(txn);
            if (err != 0)
                throw new MdbxException("mdbx_txn_reset", err);
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int RenewDelegate(IntPtr txn);

        private static RenewDelegate _renewDelegate = null;

        internal static void Renew(IntPtr txn)
        {
            int err = _renewDelegate(txn);
            if (err != 0)
                throw new MdbxException("mdbx_txn_renew", err);
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong GetTxnIdDelegate(IntPtr txn);

        private static GetTxnIdDelegate _getTxnIdDelegate = null;

        internal static ulong GetID(IntPtr txn)
        {
            return _getTxnIdDelegate(txn);
        }


        internal static void Bind()
        {
            _beginDelegate = Library.GetProcAddress<BeginDelegate>("mdbx_txn_begin") as BeginDelegate;
            _commitDelegate = Library.GetProcAddress<CommitDelegate>("mdbx_txn_commit") as CommitDelegate;
            _abortDelegate = Library.GetProcAddress<AbortDelegate>("mdbx_txn_abort") as AbortDelegate;
            _resetDelegate = Library.GetProcAddress<ResetDelegate>("mdbx_txn_reset") as ResetDelegate;
            _renewDelegate = Library.GetProcAddress<RenewDelegate>("mdbx_txn_renew") as RenewDelegate;
            _getTxnIdDelegate = Library.GetProcAddress<GetTxnIdDelegate>("mdbx_txn_id") as GetTxnIdDelegate;
        }
    }
}
