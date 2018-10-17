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
                throw new MdbxException("mdbx_env_close_ex", err);
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



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetMaxDbsDelegate(IntPtr env
            , [MarshalAs(UnmanagedType.U4)] uint dbs
            );
        private static SetMaxDbsDelegate _setMaxDbsDelegate = null;

        public static void SetMaxDBs(IntPtr env, uint dbs)
        {
            int err = _setMaxDbsDelegate(env, dbs);
            if (err != 0)
                throw new MdbxException("mdbx_env_set_maxdbs", err);
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



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetFlagsDelegate(IntPtr env, uint flags, int onoff);
        private static SetFlagsDelegate _setFlagsDelegate = null;

        public static void SetFlags(IntPtr env, EnvironmentFlag flags, bool onoff)
        {
            int err = _setFlagsDelegate(env, (uint)flags, onoff ? 1 : 0);
            if (err != 0)
                throw new MdbxException("mdbx_env_set_flags", err);
        }



        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetFlagsDelegate(IntPtr env, out uint flags);
        private static GetFlagsDelegate _getFlagsDelegate = null;

        public static EnvironmentFlag GetFlags(IntPtr env)
        {
            uint flags;
            int err = _getFlagsDelegate(env, out flags);
            if (err != 0)
                throw new MdbxException("mdbx_env_set_flags", err);
            return (EnvironmentFlag)flags;
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetMapSizeDelegate(IntPtr env, UIntPtr size);
        private static SetMapSizeDelegate _setMapSizeDelegate = null;

        public static void SetMapSize(IntPtr env, uint size)
        {
            int err = _setMapSizeDelegate(env, UIntPtr.Add(UIntPtr.Zero, (int)size));
            if (err != 0)
                throw new MdbxException("mdbx_env_set_mapsize", err);
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SetMaxReadersDelegate(IntPtr env, uint readers);
        private static SetMaxReadersDelegate _setMaxReadersDelegate = null;

        public static void SetMaxReaders(IntPtr env, uint readers)
        {
            int err = _setMaxReadersDelegate(env, readers);
            if (err != 0)
                throw new MdbxException("mdbx_env_set_maxreaders", err);
        }

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetMaxReadersDelegate(IntPtr env, out uint readers);
        private static GetMaxReadersDelegate _getMaxReadersDelegate = null;

        public static int GetMaxReaders(IntPtr env)
        {
            uint readers;
            int err = _getMaxReadersDelegate(env, out readers);
            if (err != 0)
                throw new MdbxException("mdbx_env_get_maxreaders", err);
            return (int)readers;
        }


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetMaxKeySizeDelegate(IntPtr env);
        private static GetMaxKeySizeDelegate _getMaxKeySizeDelegate = null;

        public static int GetMaxKeySize(IntPtr env)
        {
            return _getMaxKeySizeDelegate(env);
        }


        internal static void Bind()
        {
            _createDelegate = Library.GetProcAddress<CreateDelegate>("mdbx_env_create") as CreateDelegate;
            _closeDelegate = Library.GetProcAddress<CloseDelegate>("mdbx_env_close") as CloseDelegate;
            _closeExDelegate = Library.GetProcAddress<CloseExDelegate>("mdbx_env_close_ex") as CloseExDelegate;
            _openDelegate = Library.GetProcAddress<OpenDelegate>("mdbx_env_open") as OpenDelegate;
            _statDelegate = Library.GetProcAddress<StatDelegate>("mdbx_env_stat") as StatDelegate;
            _infoDelegate = Library.GetProcAddress<InfoDelegate>("mdbx_env_info") as InfoDelegate;
            _syncDelegate = Library.GetProcAddress<SyncDelegate>("mdbx_env_sync") as SyncDelegate;
            _setMaxDbsDelegate = Library.GetProcAddress<SetMaxDbsDelegate>("mdbx_env_set_maxdbs") as SetMaxDbsDelegate;
            _setFlagsDelegate = Library.GetProcAddress<SetFlagsDelegate>("mdbx_env_set_flags") as SetFlagsDelegate;
            _getFlagsDelegate = Library.GetProcAddress<GetFlagsDelegate>("mdbx_env_get_flags") as GetFlagsDelegate;
            _setMapSizeDelegate = Library.GetProcAddress<SetMapSizeDelegate>("mdbx_env_set_mapsize") as SetMapSizeDelegate;
            _setMaxReadersDelegate = Library.GetProcAddress<SetMaxReadersDelegate>("mdbx_env_set_maxreaders") as SetMaxReadersDelegate;
            _getMaxReadersDelegate = Library.GetProcAddress<GetMaxReadersDelegate>("mdbx_env_get_maxreaders") as GetMaxReadersDelegate;
            _getMaxKeySizeDelegate = Library.GetProcAddress<GetMaxKeySizeDelegate>("mdbx_env_get_maxkeysize") as GetMaxKeySizeDelegate;
        }

    }
}
