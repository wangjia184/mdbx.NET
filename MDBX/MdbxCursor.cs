using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    using Interop;

    public class MdbxCursor : IDisposable
    {
        private bool closed = false;

        public void Dispose()
        {
            //Dispose(true);
        }

        private readonly MdbxEnvironment _env;
        private readonly MdbxTransaction _tran;
        private readonly MdbxDatabase _db;
        private readonly IntPtr _cursorPtr;

        internal MdbxCursor(MdbxEnvironment env, MdbxTransaction tran, MdbxDatabase db, IntPtr cursorPtr)
        {
            _env = env;
            _tran = tran;
            _db = db;
            _cursorPtr = cursorPtr;
        }

        /// <summary>
        /// Close a cursor handle.
        /// 
        /// The cursor handle will be freed and must not be used again after this call.
        /// Its transaction must still be live if it is a write-transaction.
        /// </summary>
        void Close()
        {
            if(!closed)
            {
                closed = true;
                Cursor.Close(_cursorPtr);
            }
        }
    }
}
