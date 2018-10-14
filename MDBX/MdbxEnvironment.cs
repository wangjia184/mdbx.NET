using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    using Interop;

    public class MdbxEnvironment : IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        private bool closed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                Close();

                disposedValue = true;
            }
        }

        // override a finalizer because Dispose(bool disposing) above has code to free unmanaged resources.
        ~MdbxEnvironment() {
           // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
           Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        internal readonly IntPtr _envPtr = IntPtr.Zero;

        static MdbxEnvironment()
        {
            Library.Load();
        }

        public MdbxEnvironment()
        {
            _envPtr = Env.Create();
        }

        public void Close(bool dontSync = false)
        {
            if(!closed && _envPtr != IntPtr.Zero)
            {
                closed = true;
                Env.Close(_envPtr, dontSync);
            }
        }

        /// <summary>
        /// Open an environment handle.
        /// 
        /// This function allocates memory for a MDBX_env structure. To release
        /// the allocated memory and discard the handle, call mdbx_env_close().
        /// possible exceptions are:
        ///    - MDBX_VERSION_MISMATCH - the version of the MDBX library doesn't match the
        ///                              version that created the database environment.
        ///    - MDBX_INVALID  - the environment file headers are corrupted.
        ///    - MDBX_ENOENT   - the directory specified by the path parameter
        ///                      doesn't exist.
        ///    - MDBX_EACCES   - the user didn't have permission to access
        ///                      the environment files.
        ///    - MDBX_EAGAIN   - the environment was locked by another process. 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="flags"></param>
        /// <param name="mode"></param>
        public void Open(string path, EnvironmentFlag flags, int mode)
        {
            Env.Open(_envPtr, path, flags, mode);
        }

        /// <summary>
        /// Open an environment handle.
        /// 
        /// This function allocates memory for a MDBX_env structure. To release
        /// the allocated memory and discard the handle, call mdbx_env_close().
        /// possible exceptions are:
        ///    - MDBX_VERSION_MISMATCH - the version of the MDBX library doesn't match the
        ///                              version that created the database environment.
        ///    - MDBX_INVALID  - the environment file headers are corrupted.
        ///    - MDBX_ENOENT   - the directory specified by the path parameter
        ///                      doesn't exist.
        ///    - MDBX_EACCES   - the user didn't have permission to access
        ///                      the environment files.
        ///    - MDBX_EAGAIN   - the environment was locked by another process. 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="flags"></param>
        /// <param name="mode"></param>
        /// <param name="exclusive"></param>
        public void Open(string path, EnvironmentFlag flags, int mode, ref int exclusive)
        {
            Env.OpenEx(_envPtr, path, flags, mode, ref exclusive);
        }

        /// <summary>
        /// Create a transaction for use with the environment.
        /// 
        /// The transaction handle may be discarded using Abort() or Commit();
        /// NOTE: A transaction and its cursors must only be used by a single
        /// thread, and a thread may only have a single transaction at a time.
        /// If MDBX_NOTLS is in use, this does not apply to read-only transactions.
        /// NOTE: Cursors may not span transactions.
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public MdbxTransaction BeginTransaction(TransactionOption flags = TransactionOption.Unspecific)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                IntPtr ptr = Txn.Begin(_envPtr, IntPtr.Zero, flags);
                return new MdbxTransaction(this, ptr);
            }
            throw new InvalidOperationException("MDBX environment is not open.");            
        }

        /// <summary>
        /// Return information about the MDBX environment.
        /// </summary>
        /// <returns></returns>
        public EnvInfo Info()
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                return Env.Info(_envPtr);
            }
            throw new InvalidOperationException("MDBX environment is not open.");
        }

        /// <summary>
        /// Return statistics about the MDBX environment.
        /// </summary>
        /// <returns></returns>
        public EnvStat Stat()
        {
            if( !closed && _envPtr != IntPtr.Zero)
            {
                return Env.Stat(_envPtr);
            }
            throw new InvalidOperationException("MDBX environment is not open.");
        }


        /// <summary>
        /// Flush the data buffers to disk.
        /// 
        /// Data is always written to disk when mdbx_txn_commit() is called,
        /// but the operating system may keep it buffered. MDBX always flushes
        /// the OS buffers upon commit as well, unless the environment was
        /// opened with MDBX_NOSYNC or in part MDBX_NOMETASYNC. This call is
        /// not valid if the environment was opened with MDBX_RDONLY.
        /// </summary>
        /// <param name="force">
        /// If non-zero, force a synchronous flush.  Otherwise if the
        /// environment has the MDBX_NOSYNC flag set the flushes will be
        /// omitted, and with MDBX_MAPASYNC they will be asynchronous.
        /// </param>
        public void Sync(bool force)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Env.Sync(_envPtr, force);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }
    }
}
