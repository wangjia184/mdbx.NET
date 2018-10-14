using System;
using System.Collections.Generic;
using System.Text;

namespace MDBX
{
    using Interop;

    public class MdbxTransaction : IDisposable
    {
        #region IDisposable Support
        public void Dispose()
        {
            Abort();
        }
        #endregion


        private bool _released = false;

        private readonly MdbxEnvironment _env;
        internal readonly IntPtr _txnPtr;

        internal MdbxTransaction(MdbxEnvironment env, IntPtr txnPtr)
        {
            _env = env;
            _txnPtr = txnPtr;
        }

        /// <summary>
        ///  Commit all the operations of a transaction into the database.
        ///  
        /// The transaction handle is freed. It and its cursors must not be used
        /// again after this call, except with mdbx_cursor_renew().
        /// </summary>
        public void Commit()
        {
            if (!_released)
            {
                _released = true;
                Txn.Commit(_txnPtr);
            }
            else
            {
                throw new InvalidOperationException("MDBX transaction handle was freed. It can't be used again unless Reset() is called");
            }
        }

        /// <summary>
        /// Abandon all the operations of the transaction instead of saving them.
        /// </summary>
        public void Abort()
        {
            if(!_released)
            {
                _released = true;
                Txn.Abort(_txnPtr);
            }
        }

        /// <summary>
        /// Reset a read-only transaction.
        /// 
        /// Abort the transaction like Abort(), but keep the transaction
        /// handle. Therefore Renew() may reuse the handle. This saves
        /// allocation overhead if the process will start a new read-only transaction
        /// soon, and also locking overhead if MDBX_NOTLS is in use. The reader table
        /// lock is released, but the table slot stays tied to its thread or
        /// MDBX_txn. Use mdbx_txn_abort() to discard a reset handle, and to free
        /// its lock table slot if MDBX_NOTLS is in use.
        /// </summary>
        public void Reset()
        {
            Txn.Reset(_txnPtr);
        }

        /// <summary>
        /// Renew a read-only transaction.
        /// 
        /// This acquires a new reader lock for a transaction handle that had been
        /// released by mdbx_txn_reset(). It must be called before a reset transaction
        /// may be used again.
        /// </summary>
        public void Renew()
        {
            Txn.Renew(_txnPtr);
        }

        /// <summary>
        /// Open a table in the environment.
        /// 
        /// A table handle denotes the name and parameters of a table, independently
        /// of whether such a table exists. The table handle may be discarded by
        /// calling mdbx_dbi_close(). The old table handle is returned if the table
        /// was already open. The handle may only be closed once.
        /// 
        /// The table handle will be private to the current transaction until
        /// the transaction is successfully committed. If the transaction is
        /// aborted the handle will be closed automatically.
        /// After a successful commit the handle will reside in the shared
        /// environment, and may be used by other transactions.
        /// 
        /// This function must not be called from multiple concurrent
        /// transactions in the same process. A transaction that uses
        /// this function must finish (either commit or abort) before
        /// any other transaction in the process may use this function.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public MdbxDatabase OpenDatabase(string name = null, DatabaseOption option = DatabaseOption.Unspecific)
        {
            return new MdbxDatabase(_env, this, Dbi.Open(_txnPtr, name, option));
        }

    }
}
