using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;

namespace MDBX.Interop
{
    internal static class Misc
    {
        /// <summary>
        /// char *mdbx_strerror_r(int errnum, char *buf, size_t buflen)
        /// </summary>
        /// <param name="err"></param>
        /// <param name="buf"></param>
        /// <param name="buflen"></param>
        /// <returns></returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr StringErrorDelegate(int err);

        private static StringErrorDelegate _stringErrorDelegate = null;

        internal static string StringError(int err)
        {
            IntPtr ptr = _stringErrorDelegate(err);
            return Marshal.PtrToStringAnsi(ptr);
        }

        

        internal static void Bind()
        {
            _stringErrorDelegate = Library.GetProcAddress<StringErrorDelegate>("mdbx_strerror") as StringErrorDelegate;
        }

    }
}
