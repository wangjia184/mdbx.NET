using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    internal static class Env
    {
        /// <summary>
        /// int mdbx_env_create(MDBX_env **penv)
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CreateDelegate(out IntPtr env);

        private static CreateDelegate _createDelegate = null;

        internal static IntPtr Create()
        {
            IntPtr ptr;
            int err = _createDelegate(out ptr);
            if (err != 0)
                throw new MdbxException("mdbx_env_create", err);
            return ptr;
        }

        /// <summary>
        /// int mdbx_env_close(MDBX_env *env)
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CloseDelegate(IntPtr env);

        private static CloseDelegate _closeDelegate = null;

        internal static void Close(IntPtr env)
        {
            int err = _closeDelegate(env);
            if (err != 0)
                throw new MdbxException("mdbx_env_close", err);
        }


        /// <summary>
        /// int mdbx_env_close_ex(MDBX_env *env, int dont_sync);
        /// </summary>
        /// <param name="env"></param>
        /// <param name="dont_sync"></param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int CloseExDelegate(IntPtr env, [MarshalAs(UnmanagedType.I4)] int dont_sync);

        private static CloseExDelegate _closeExDelegate = null;

        internal static void Close(IntPtr env, bool dontSync)
        {
            int err = _closeExDelegate(env, dontSync ? 1 : 0);
            if (err != 0)
                throw new MdbxException("mdbx_env_open_ex", err);
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int OpenDelegate(IntPtr env
            , [MarshalAs(UnmanagedType.LPStr)] string path
            , [MarshalAs(UnmanagedType.U4)] int flags
            , [MarshalAs(UnmanagedType.I4)] int mode
            );
        private static OpenDelegate _openDelegate = null;

        public static void Open(IntPtr env, string path, EnvironmentFlag flags, int mode)
        {
            int err = _openDelegate(env, path, (int)flags, mode);
            if (err != 0)
                throw new MdbxException("mdbx_env_open", err);
        }



        /// <summary>
        /// int mdbx_env_open_ex(MDBX_env *env, const char *path, unsigned flags, mode_t mode, int* exclusive);
        /// </summary>
        /// <param name="env"></param>
        /// <param name="path"></param>
        /// <param name="flags"></param>
        /// <param name="mode"></param>
        /// <param name="exclusive"></param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int OpenExDelegate(IntPtr env
            , [MarshalAs(UnmanagedType.LPStr)] string path
            , [MarshalAs(UnmanagedType.U4)] int flags
            , [MarshalAs(UnmanagedType.I4)] int mode
            , ref int exclusive
            );
        private static OpenExDelegate _openExDelegate = null;

        public static void OpenEx(IntPtr env, string path, EnvironmentFlag flags, int mode, ref int exclusive)
        {
            int err = _openExDelegate(env, path, (int)flags, mode, ref exclusive);
            if (err != 0)
                throw new MdbxException("mdbx_env_close_ex", err);
        }




        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int StatDelegate(IntPtr env
            , ref EnvStat stat
            , UIntPtr bytes
            );
        private static StatDelegate _statDelegate = null;

        public static EnvStat Stat(IntPtr env)
        {
            EnvStat stat = new EnvStat();
            UIntPtr bytes = UIntPtr.Add(UIntPtr.Zero, Marshal.SizeOf(stat));
            int err = _statDelegate(env, ref stat, bytes);
            if (err != 0)
                throw new MdbxException("mdbx_env_stat", err);
            return stat;
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int InfoDelegate(IntPtr env
            , ref EnvInfo info
            , UIntPtr bytes
            );
        private static InfoDelegate _infoDelegate = null;

        public static EnvInfo Info(IntPtr env)
        {
            EnvInfo info = new EnvInfo();
            UIntPtr bytes = UIntPtr.Add(UIntPtr.Zero, Marshal.SizeOf(info));
            int err = _infoDelegate(env, ref info, bytes);
            if (err != 0)
                throw new MdbxException("mdbx_env_info", err);
            return info;
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SyncDelegate(IntPtr env, int force);
        private static SyncDelegate _syncDelegate = null;

        public static void Sync(IntPtr env, bool force)
        {
            int err = _syncDelegate(env, force ? 1 : 0);
            if (err != 0)
                throw new MdbxException("mdbx_env_sync", err);
        }


        internal static void Bind()
        {
            _createDelegate = Library.GetProcAddress<CreateDelegate>("mdbx_env_create") as CreateDelegate;
            _closeDelegate = Library.GetProcAddress<CloseDelegate>("mdbx_env_close") as CloseDelegate;
            _closeExDelegate = Library.GetProcAddress<CloseExDelegate>("mdbx_env_close_ex") as CloseExDelegate;
            _openExDelegate = Library.GetProcAddress<OpenExDelegate>("mdbx_env_open_ex") as OpenExDelegate;
            _openDelegate = Library.GetProcAddress<OpenDelegate>("mdbx_env_open") as OpenDelegate;
            _statDelegate = Library.GetProcAddress<StatDelegate>("mdbx_env_stat") as StatDelegate;
            _infoDelegate = Library.GetProcAddress<InfoDelegate>("mdbx_env_info") as InfoDelegate;
            _syncDelegate = Library.GetProcAddress<SyncDelegate>("mdbx_env_sync") as SyncDelegate;
        }

    }
}
