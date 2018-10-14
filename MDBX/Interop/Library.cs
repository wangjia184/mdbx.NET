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
            string dir = null;
            string filename = null;
            if( RuntimeInformation.IsOSPlatform(OSPlatform.Windows) )
            {
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X64:
                        dir = "x64";
                        filename = "mdbx.dll";
                        break;

                    case Architecture.X86:
                        dir = "x86";
                        filename = "mdbx.dll";
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(dir))
                throw new PlatformNotSupportedException($"{RuntimeInformation.OSDescription} {RuntimeInformation.ProcessArchitecture.ToString()} is not supported");

            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            filename = Path.Combine(folder, dir, filename);

            if (!File.Exists(filename))
                throw new FileNotFoundException("MDBX failed to load.", filename);


            _libPtr = LoadLibrary(filename);

            Misc.Bind();
            Env.Bind();
            Txn.Bind();
            Dbi.Bind();
            Cursor.Bind();
        }

    }
}
