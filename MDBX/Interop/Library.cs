using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MDBX.Interop
{
    internal static class Library
    {
        [DllImport("libdl.so")]
        private static extern IntPtr dlopen(string filename, int flags);

        [DllImport("libdl.so")]
        private static extern IntPtr dlsym(IntPtr handle, string symbol);

        const int RTLD_NOW = 2; // for dlopen's flags 


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        private static IntPtr _libPtr = IntPtr.Zero;


        internal static Delegate GetProcAddress<T>(string procName)
        {
            IntPtr ptr = IntPtr.Zero;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ptr = GetProcAddress(_libPtr, procName);
            else
                ptr = dlsym(_libPtr, procName);
            if (ptr != IntPtr.Zero)
            {
                return Marshal.GetDelegateForFunctionPointer(ptr, typeof(T));
            }

            throw new BadImageFormatException($"MDBX failed to bind '{procName}' function.");
        }

        internal static void Load()
        {
            string platform = null;
            string filename = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                platform = "windows";
                filename = "mdbx.dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                platform = "linux";
                filename = "mdbx.so";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                platform = "osx";
                filename = "mdbx.so";
            }
            else
                throw new PlatformNotSupportedException($"Unsupported OS platform : {RuntimeInformation.OSDescription}");

            string filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                , "native"
                , platform.ToLowerInvariant()
                , RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant()
                , filename
                );

            if (!File.Exists(filepath))
                throw new FileNotFoundException($"MDBX cannot find the library at {filepath}", filepath);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                _libPtr = LoadLibrary(filepath);
            else
                _libPtr = dlopen(filepath, RTLD_NOW);

            if(_libPtr == IntPtr.Zero )
                throw new FileNotFoundException($"MDBX failed to load library at {filepath}", filepath);

            Misc.Bind();
            Env.Bind();
            Txn.Bind();
            Dbi.Bind();
            Cursor.Bind();
        }

    }
}
