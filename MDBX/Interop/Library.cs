using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MDBX.Interop
{
    internal static class Library
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        private static IntPtr _libPtr = IntPtr.Zero;


        internal static Delegate GetProcAddress<T>(string procName)
        {
            IntPtr ptr = GetProcAddress(_libPtr, procName);
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


            _libPtr = LoadLibrary(filepath);

            Misc.Bind();
            Env.Bind();
            Txn.Bind();
            Dbi.Bind();
            Cursor.Bind();
        }

    }
}
