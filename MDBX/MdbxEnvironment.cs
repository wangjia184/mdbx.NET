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

        /// <summary>
        /// Set the maximum number of named databases for the environment.
        /// This function is only needed if multiple databases will be used in the
        /// environment. Simpler applications that use the environment as a single
        /// unnamed database can ignore this option.
        /// 
        /// This function may only be called after mdbx_env_create() and before
        /// mdbx_env_open().
        /// </summary>
        /// <param name="num"></param>
        public MdbxEnvironment SetMaxDatabases(uint num)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Env.SetMaxDBs(_envPtr, num);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
            return this;
        }

        /// <summary>
        /// Set the maximum number of threads/reader slots for the environment.
        /// 
        /// This defines the number of slots in the lock table that is used to track
        /// readers in the the environment. The default is 61.
        /// Starting a read-only transaction normally ties a lock table slot to the
        /// current thread until the environment closes or the thread exits. If
        /// MDBX_NOTLS is in use, mdbx_txn_begin() instead ties the slot to the
        /// MDBX_txn object until it or the MDBX_env object is destroyed.
        /// This function may only be called after mdbx_env_create() and before Open()
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public MdbxEnvironment SetMaxReaders(uint num)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Env.SetMaxReaders(_envPtr, num);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
            return this;
        }

        /// <summary>
        /// Set the size of the memory map to use for this environment.
        /// 
        /// The size should be a multiple of the OS page size. The default is
        /// 10485760 bytes. The size of the memory map is also the maximum size
        /// of the database. The value should be chosen as large as possible,
        /// to accommodate future growth of the database.
        /// 
        /// This function should be called before Open()
        /// It may be called at later times if no transactions
        /// are active in this process. Note that the library does not check for
        /// this condition, the caller must ensure it explicitly.
        /// 
        /// The new size takes effect immediately for the current process but
        /// will not be persisted to any others until a write transaction has been
        /// committed by the current process. Also, only mapsize increases are
        /// persisted into the environment.
        /// 
        /// If the mapsize is increased by another process, and data has grown
        /// beyond the range of the current mapsize, mdbx_txn_begin() will
        /// return MDBX_MAP_RESIZED. This function may be called with a size
        /// of zero to adopt the new size.
        /// 
        /// Any attempt to set a size smaller than the space already consumed by the
        /// environment will be silently changed to the current size of the used space.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public MdbxEnvironment SetMapSize(uint num)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Env.SetMapSize(_envPtr, num);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
            return this;
        }


        public void SetFlags(EnvironmentFlag flags, SetOption option = SetOption.Add)
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                Env.SetFlags(_envPtr, flags, option == SetOption.Add);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }

        public EnvironmentFlag GetFlags()
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                return Env.GetFlags(_envPtr);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }

        /// <summary>
        /// Get the maximum number of threads/reader slots for the environment.
        /// </summary>
        /// <returns></returns>
        public int GetMaxReaders()
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                return Env.GetMaxReaders(_envPtr);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }

        /// <summary>
        /// Get the maximum size of keys and MDBX_DUPSORT data we can write.
        /// </summary>
        /// <returns></returns>
        public int GetMaxKeySize()
        {
            if (!closed && _envPtr != IntPtr.Zero)
            {
                return Env.GetMaxKeySize(_envPtr);
            }
            else
            {
                throw new InvalidOperationException("MDBX environment is not open.");
            }
        }
    }
}
