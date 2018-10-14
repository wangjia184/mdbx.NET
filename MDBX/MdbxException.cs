using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    using Interop;

    public class MdbxException : Exception
    {
        public int ErrorNumber { get { return _errorNumber; } }
        private readonly int _errorNumber;
        internal MdbxException(string method, int errNum) : 
            base(GetMessage(method, errNum))
        {
            _errorNumber = errNum;
        }

        private static string GetMessage(string method, int errNum)
        {
            return string.Format("MDBX {0} returned ({1}) - {2}"
                , method
                , errNum
                , Misc.StringError(errNum)
                );
        }
    }
}
